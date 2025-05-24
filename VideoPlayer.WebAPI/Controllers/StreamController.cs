using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Ffmpeg;

namespace VideoPlayer.WebAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class StreamController : ControllerBase
  {
    private readonly IVideoRepository _videoRepository;
    private readonly IFfmpegStreamer _ffmpegStreamer;
    private readonly IFfprobeInfoExtractor _ffprobeInfoExtractor;

    private static IDictionary<Int32, FfprobeVideoInfo> _videoInfoCache = new ConcurrentDictionary<Int32, FfprobeVideoInfo>();

    public StreamController(IVideoRepository videoRepository, IFfmpegStreamer ffmpegStreamer, IFfprobeInfoExtractor ffprobeInfoExtractor)
    {
      this._videoRepository = videoRepository;
      this._ffmpegStreamer = ffmpegStreamer;
      this._ffprobeInfoExtractor = ffprobeInfoExtractor;
    }

    [HttpGet("transcode/{id}")]
    public async Task<IActionResult> TranscodeAndStreamWithRange(Int32 id)
    {
      var video = this._videoRepository.Get(id);
      var videoPath = video.FileName;

      if (!System.IO.File.Exists(videoPath))
      {
        return this.NotFound();
      }

      // Get or cache video info
      if (!_videoInfoCache.ContainsKey(id))
      {
        _videoInfoCache.Add(id, this._ffprobeInfoExtractor.GetVideoInfo(videoPath));
      }
      var videoInfo = _videoInfoCache[id];

      // Parse file size from ffprobe output
      var totalLength = Int64.Parse(videoInfo.format.size);
      
      this.Response.Headers.Append("Accept-Ranges", "bytes");
      this.Response.Headers.Append("Content-Type", "video/mp4");

      var range = this.Request.Headers.Range.FirstOrDefault();
      Int64 startByte = 0;
      Int64 endByte = totalLength - 1;
      
      if (!String.IsNullOrEmpty(range))
      {
          var parts = range.Split('=').Last().Split('-');
          if (parts.Length > 0 && !String.IsNullOrEmpty(parts[0]))
          {
              startByte = Int64.Parse(parts[0]);
              if (parts.Length > 1 && !String.IsNullOrEmpty(parts[1]))
              {
                  endByte = Int64.Parse(parts[1]);
              }
          }
      }

      try
      {
        var stream = await this._ffmpegStreamer.Stream(videoPath, startByte);
        if (stream == null)
        {
            return StatusCode(500, "Failed to create video stream");
        }

        if (startByte > 0 || endByte < totalLength - 1)
        {
          this.Response.Headers.Append("Content-Range", $"bytes {startByte}-{endByte}/{totalLength}");
          this.Response.Headers.Append("Content-Length", (endByte - startByte + 1).ToString());
          this.Response.StatusCode = 206;
        }
        else
        {
          this.Response.Headers.Append("Content-Length", totalLength.ToString());
        }

        if (!String.IsNullOrEmpty(videoInfo.format.duration))
        {
          var durationInSeconds = Double.Parse(videoInfo.format.duration);
          this.Response.Headers.Append("X-Content-Duration", durationInSeconds.ToString());
        }

        return new FileStreamResult(stream, "video/mp4")
        {
          EnableRangeProcessing = true
        };
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Transcoding failed: {ex.Message}");
      }
    }
  }
}