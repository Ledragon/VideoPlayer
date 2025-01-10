
public class StreamTranscoder : IStreamTranscoder
{
    private readonly ILogger<StreamTranscoder> _logger;
    private readonly TranscodeContext _context;
    private readonly FFmpegOptions _options;

    public StreamTranscoder(
        ILogger<StreamTranscoder> logger,
        TranscodeContext context,
        IOptions<FFmpegOptions> options)
    {
        _logger = logger;
        _context = context;
        _options = options.Value;
    }

    public async Task<RequestDelegate> CreateTranscodeHandlerAsync(TranscodeOptions options)
    {
        var cmd = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = _options.FFmpegPath,
                Arguments = BuildFFmpegArguments(options),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        try
        {
            await StartAndMonitorProcessAsync(cmd);
            _context.AttachProcess(cmd);

            return BuildResponseHandler(cmd, options);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[transcode] Failed to start FFmpeg: {Error}", ex.Message);
            throw;
        }
    }

    private async Task StartAndMonitorProcessAsync(Process process)
    {
        if (!process.Start())
        {
            throw new InvalidOperationException("Failed to start FFmpeg process");
        }

        // Monitor stderr in background
        _ = Task.Run(async () =>
        {
            try
            {
                using var reader = process.StandardError;
                var error = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(error))
                {
                    _logger.LogError("[transcode] FFmpeg error: {Error}", error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[transcode] Error monitoring FFmpeg process");
            }
        });
    }

    private static RequestDelegate BuildResponseHandler(Process cmd, TranscodeOptions options)
    {
        return async context =>
        {
            context.Response.Headers.CacheControl = "no-store";
            context.Response.Headers.ContentType = options.StreamType.MimeType;
            context.Response.StatusCode = StatusCodes.Status200OK;

            try
            {
                await cmd.StandardOutput.BaseStream.CopyToAsync(context.Response.Body);
                await context.Response.Body.FlushAsync();
            }
            catch (IOException ex) when (IsConnectionAborted(ex))
            {
                // Client disconnected, ignore
            }
        };
    }

    private string BuildFFmpegArguments(TranscodeOptions options)
    {
        var args = new List<string>();

        // Input file
        args.AddRange(new[] { "-i", options.VideoFile.Path });

        // Add start time if specified
        if (options.StartTime > 0)
        {
            args.AddRange(new[] { "-ss", options.StartTime.ToString(CultureInfo.InvariantCulture) });
        }

        // Add stream format specific arguments
        args.AddRange(options.StreamType.Args(
            options.VideoFile.VideoCodec,
            new VideoFilter("scale", GetScaleFilter(options)),
            false));

        return string.Join(" ", args);
    }

    private string GetScaleFilter(TranscodeOptions options)
    {
        if (string.IsNullOrEmpty(options.Resolution))
            return "-1";

        return $"min(iw\\,{options.Resolution}):-1";
    }

  private static bool IsConnectionAborted(IOException ex) =>
      ex.InnerException is SocketException;//|| ex.InnerException is PipeException;

}