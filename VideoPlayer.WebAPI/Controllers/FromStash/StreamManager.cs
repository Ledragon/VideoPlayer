public class StreamManager : IStreamManager
{
    private readonly ILogger<StreamManager> _logger;
    private readonly IStreamTranscoder _transcoder;

    public StreamManager(
        ILogger<StreamManager> logger,
        IStreamTranscoder transcoder)
    {
        _logger = logger;
        _transcoder = transcoder;
    }

    public Boolean IsTranscodingEnabled => throw new NotImplementedException();


    public async Task ServeTranscodeAsync(HttpContext context, TranscodeOptions options)
    {
        var handler = await _transcoder.CreateTranscodeHandlerAsync(options);
        await handler(context);
    }
}