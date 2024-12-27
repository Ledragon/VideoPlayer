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

    // Create an endpoint to get video metadata. Use the videoRepository to get the video by ID. Metadata should be usable by a video html element preload="metadata"



    [HttpGet("transcode/{id}")]
    public async Task<IActionResult> TranscodeAndStreamWithRange(Int32 id)
    {
      var video = this._videoRepository.Get(id);
      var videoPath = video.FileName;

      if (!System.IO.File.Exists(videoPath))
      {
        return this.NotFound();
      }

      this.Response.Headers.Append("Accept-Ranges", "bytes");
      this.Response.Headers.Append("Content-Type", "video/mp4");

      var range = this.Request.Headers.Range.FirstOrDefault()?.Split('=').Last().Split('-');
      var startByte = range != null ? Int64.Parse(range[0]) : 0;
      try
      {
        var stream = await this._ffmpegStreamer.Stream(videoPath, startByte);

        if (startByte > 0)
        {
          this.Response.StatusCode = 206;
        }
        // try
        // {
        if(!_videoInfoCache.ContainsKey(id))
        {
          _videoInfoCache.Add(id, this._ffprobeInfoExtractor.GetVideoInfo(videoPath));
        }
        var metaData = _videoInfoCache[id];
        Int64 length = Convert.ToInt64(metaData.format.size);
        this.Response.Headers.Append("Content-Length", (length + 1).ToString());
        // var contentRangeHeaderValue = new ContentRangeHeaderValue(startByte, length - startByte - 1, length);
        // this.Response.Headers.ContentRange = contentRangeHeaderValue.ToString();
        // }
        // catch (Exception ex)
        // {
        // return StatusCode(500, $"Transcoding failed: {ex.Message}");
        // }
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