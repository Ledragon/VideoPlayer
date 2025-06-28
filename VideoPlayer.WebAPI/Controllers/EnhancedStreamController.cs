using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Ffmpeg;

namespace VideoPlayer.WebAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class EnhancedStreamController : ControllerBase
  {
    private readonly IVideoRepository _videoRepository;
    private readonly IAdvancedFfmpegStreamer _ffmpegStreamer;
    private readonly IFfprobeInfoExtractor _ffprobeInfoExtractor;
    private readonly String _cacheDirectory;

    private static readonly ConcurrentDictionary<Int32, FfprobeVideoInfo> _videoInfoCache = new();
    private static readonly ConcurrentDictionary<String, String> _transcodingCache = new();

    public EnhancedStreamController(
        IVideoRepository videoRepository,
        IAdvancedFfmpegStreamer ffmpegStreamer,
        IFfprobeInfoExtractor ffprobeInfoExtractor,
        IConfiguration configuration)
    {
      this._videoRepository = videoRepository;
      this._ffmpegStreamer = ffmpegStreamer;
      this._ffprobeInfoExtractor = ffprobeInfoExtractor;

      this._cacheDirectory = configuration.GetValue<String>("TranscodingCache:Directory") ??
                           Path.Combine(Path.GetTempPath(), "VideoPlayerCache");

      Directory.CreateDirectory(this._cacheDirectory);
    }

    [HttpGet("stream/{id}")]
    public async Task<IActionResult> StreamVideo(
        Int32 id,
        [FromQuery] Double seek = 0,
        [FromQuery] Double speed = 1.0,
        [FromQuery] String quality = "medium",
        [FromQuery] Boolean useCache = true)
    {
      var video = this._videoRepository.Get(id);
      if (video == null)
        return this.NotFound("Video not found");

      if (!System.IO.File.Exists(video.FileName))
        return this.NotFound("Video file not found");

      try
      {
        // Check cache first
        if (useCache && Math.Abs(seek) < 0.01 && Math.Abs(speed - 1.0) < 0.01)
        {
          var cachedPath = await this.GetOrCreateCachedVideo(video.FileName, quality);
          if (!String.IsNullOrEmpty(cachedPath))
          {
            return await this.StreamCachedFile(cachedPath);
          }
        }

        // Live transcoding
        var stream = await this._ffmpegStreamer.StreamWithSeek(video.FileName, seek, speed, quality);
        if (stream == null)
          return this.StatusCode(500, "Failed to start transcoding");

        // Set appropriate headers
        this.Response.Headers.Append("Accept-Ranges", "bytes");
        this.Response.Headers.Append("Content-Type", "video/mp4");
        this.Response.Headers.Append("Cache-Control", "no-cache");

        return new FileStreamResult(stream, "video/mp4")
        {
          EnableRangeProcessing = false // Disable for transcoded streams
        };
      }
      catch (Exception ex)
      {
        return this.StatusCode(500, $"Streaming failed: {ex.Message}");
      }
    }

    [HttpGet("stream-segment/{id}")]
    public async Task<IActionResult> StreamVideoSegment(
        Int32 id,
        [FromQuery] Double start,
        [FromQuery] Double duration,
        [FromQuery] Double speed = 1.0,
        [FromQuery] String quality = "medium")
    {
      var video = this._videoRepository.Get(id);
      if (video == null)
        return this.NotFound("Video not found");

      if (!System.IO.File.Exists(video.FileName))
        return this.NotFound("Video file not found");

      try
      {
        var stream = await this._ffmpegStreamer.StreamSegment(video.FileName, start, duration, speed, quality);
        if (stream == null)
          return this.StatusCode(500, "Failed to start segment transcoding");

        this.Response.Headers.Append("Content-Type", "video/mp4");
        this.Response.Headers.Append("Cache-Control", "no-cache");

        return new FileStreamResult(stream, "video/mp4");
      }
      catch (Exception ex)
      {
        return this.StatusCode(500, $"Segment streaming failed: {ex.Message}");
      }
    }

    [HttpGet("metadata/{id}")]
    public async Task<IActionResult> GetVideoMetadata(Int32 id)
    {
      var video = this._videoRepository.Get(id);
      if (video == null)
        return this.NotFound("Video not found");

      if (!System.IO.File.Exists(video.FileName))
        return this.NotFound("Video file not found");

      try
      {
        if (!_videoInfoCache.TryGetValue(id, out var videoInfo))
        {
          videoInfo = this._ffprobeInfoExtractor.GetVideoInfo(video.FileName);
          _videoInfoCache.TryAdd(id, videoInfo);
        }

        var metadata = new
        {
          Duration = videoInfo.format.duration,
          Size = videoInfo.format.size,
          BitRate = videoInfo.format.bit_rate,
          Filename = Path.GetFileName(video.FileName),
          Streams = videoInfo.streams.Select(s => new
          {
            s.index,
            s.codec_name,
            s.codec_type,
            s.width,
            s.height,
            s.duration,
            s.bit_rate
          })
        };

        return this.Ok(metadata);
      }
      catch (Exception ex)
      {
        return this.StatusCode(500, $"Failed to get metadata: {ex.Message}");
      }
    }

    [HttpPost("pre-transcode/{id}")]
    public async Task<IActionResult> PreTranscodeVideo(Int32 id, [FromQuery] String quality = "medium")
    {
      var video = this._videoRepository.Get(id);
      if (video == null)
        return this.NotFound("Video not found");

      if (!System.IO.File.Exists(video.FileName))
        return this.NotFound("Video file not found");

      try
      {
        var cacheKey = this.GenerateCacheKey(video.FileName, quality);
        var cachedPath = Path.Combine(this._cacheDirectory, $"{cacheKey}.mp4");

        if (System.IO.File.Exists(cachedPath))
          return this.Ok(new { message = "Video already transcoded", cached = true });

        var success = await this._ffmpegStreamer.PreTranscodeVideo(video.FileName, cachedPath, quality);

        if (success)
        {
          _transcodingCache.TryAdd(cacheKey, cachedPath);
          return this.Ok(new { message = "Video transcoded successfully", cached = false });
        }

        return this.StatusCode(500, "Transcoding failed");
      }
      catch (Exception ex)
      {
        return this.StatusCode(500, $"Pre-transcoding failed: {ex.Message}");
      }
    }

    [HttpGet("hardware-info")]
    public IActionResult GetHardwareInfo()
    {
      return this.Ok(new
      {
        HardwareAccelerationAvailable = this._ffmpegStreamer.IsHardwareAccelerationAvailable(),
        SupportedQualities = new[] { "low", "medium", "high", "ultra" }
      });
    }

    [HttpPost("clear-cache")]
    public IActionResult ClearCache()
    {
      try
      {
        if (Directory.Exists(this._cacheDirectory))
        {
          var files = Directory.GetFiles(this._cacheDirectory, "*.mp4");
          foreach (var file in files)
          {
            try
            {
              System.IO.File.Delete(file);
            }
            catch (Exception ex)
            {
              // Continue with other files - could log the exception if needed
            }
          }
        }

        _transcodingCache.Clear();
        return this.Ok(new { message = "Cache cleared successfully" });
      }
      catch (Exception ex)
      {
        return this.StatusCode(500, "Failed to clear cache");
      }
    }

    private async Task<String?> GetOrCreateCachedVideo(String videoPath, String quality)
    {
      var cacheKey = this.GenerateCacheKey(videoPath, quality);

      if (_transcodingCache.TryGetValue(cacheKey, out var cachedPath) && System.IO.File.Exists(cachedPath))
        return cachedPath;

      cachedPath = Path.Combine(this._cacheDirectory, $"{cacheKey}.mp4");

      if (System.IO.File.Exists(cachedPath))
      {
        _transcodingCache.TryAdd(cacheKey, cachedPath);
        return cachedPath;
      }

      // Try to pre-transcode in background
      _ = Task.Run(async () =>
      {
        try
        {
          await this._ffmpegStreamer.PreTranscodeVideo(videoPath, cachedPath, quality);
          if (System.IO.File.Exists(cachedPath))
          {
            _transcodingCache.TryAdd(cacheKey, cachedPath);
          }
        }
        catch (Exception)
        {
          // Background transcoding failed, continue without cache
        }
      });

      return null; // Not available immediately
    }

    private Task<IActionResult> StreamCachedFile(String filePath)
    {
      var fileInfo = new FileInfo(filePath);
      var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

      this.Response.Headers.Append("Accept-Ranges", "bytes");
      this.Response.Headers.Append("Content-Type", "video/mp4");
      this.Response.Headers.Append("Cache-Control", "public, max-age=3600");

      // Handle range requests for cached files  
      var rangeHeader = this.Request.Headers.Range.FirstOrDefault()?.Split('=').LastOrDefault()?.Split('-');
      if (rangeHeader != null && rangeHeader.Length == 2)
      {
        var start = !String.IsNullOrEmpty(rangeHeader[0]) ? Int64.Parse(rangeHeader[0]) : 0;
        var end = !String.IsNullOrEmpty(rangeHeader[1]) ? Int64.Parse(rangeHeader[1]) : fileInfo.Length - 1;

        this.Response.StatusCode = 206;
        this.Response.Headers.Append("Content-Range", $"bytes {start}-{end}/{fileInfo.Length}");
        this.Response.Headers["Content-Length"] = (end - start + 1).ToString();

        fileStream.Seek(start, SeekOrigin.Begin);
        return Task.FromResult<IActionResult>(new FileStreamResult(fileStream, "video/mp4"));
      }

      this.Response.Headers.Append("Content-Length", fileInfo.Length.ToString());
      return Task.FromResult<IActionResult>(new FileStreamResult(fileStream, "video/mp4"));
    }

    private String GenerateCacheKey(String videoPath, String quality)
    {
      var input = $"{videoPath}_{quality}_{new FileInfo(videoPath).LastWriteTime:yyyyMMddHHmmss}";
      using var sha256 = SHA256.Create();
      var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
      return Convert.ToHexString(hash)[..16].ToLowerInvariant();
    }
  }
}