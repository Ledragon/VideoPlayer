using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using VideoPlayer.Database.Repository.Contracts;

namespace VideoPlayer.WebAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class StreamController : ControllerBase
  {
    private readonly IVideoRepository _videoRepository;

    public StreamController(IVideoRepository videoRepository)
    {
      this._videoRepository = videoRepository;
    }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetVideo(string id)
    // {
    //   var videoPath = Path.Combine(_environment.ContentRootPath, "Videos", $"{id}.mp4");

    //   if (!System.IO.File.Exists(videoPath))
    //     return NotFound();

    //   var fileStream = System.IO.File.OpenRead(videoPath);
    //   var mimeType = "video/mp4";

    //   // Support for range requests
    //   if (Request.Headers.Range.Count > 0)
    //   {
    //     var range = Request.Headers.Range.First().Split('=')[1].Split('-');
    //     var start = long.Parse(range[0]);
    //     var length = fileStream.Length - start;

    //     Response.StatusCode = 206;
    //     Response.Headers.Add("Accept-Ranges", "bytes");
    //     Response.Headers.Add("Content-Range", $"bytes {start}-{fileStream.Length - 1}/{fileStream.Length}");

    //     fileStream.Seek(start, SeekOrigin.Begin);
    //     return File(fileStream, mimeType, enableRangeProcessing: true);
    //   }

    //   return File(fileStream, mimeType, enableRangeProcessing: true);
    // }

    // [HttpGet("transcode/{id}")]
    // public async Task<IActionResult> TranscodeAndStream(string id)
    // {
    //   var videoPath = Path.Combine(_environment.ContentRootPath, "Videos", id);

    //   if (!System.IO.File.Exists(videoPath))
    //     return NotFound();

    //   Response.Headers.Add("Content-Type", "video/mp4");

    //   var process = new Process
    //   {
    //     StartInfo = new ProcessStartInfo
    //     {
    //       FileName = "ffmpeg",
    //       Arguments = $"-i \"{videoPath}\" -c:v libx264 -preset ultrafast -c:a aac -f mp4 pipe:1",
    //       RedirectStandardOutput = true,
    //       UseShellExecute = false,
    //       CreateNoWindow = true
    //     }
    //   };

    //   try
    //   {
    //     process.Start();
    //     return new FileStreamResult(process.StandardOutput.BaseStream, "video/mp4");
    //   }
    //   catch (Exception ex)
    //   {
    //     return StatusCode(500, $"Transcoding failed: {ex.Message}");
    //   }
    // }

    [HttpGet("transcode/{id}")]
    public async Task<IActionResult> TranscodeAndStreamWithRange(Int32 id)
    {
      var video = this._videoRepository.Get(id);
      var videoPath = video.FileName;

      if (!System.IO.File.Exists(videoPath))
        return NotFound();

      Response.Headers.Add("Accept-Ranges", "bytes");
      Response.Headers.Add("Content-Type", "video/mp4");
      Response.Headers.Append("X-Video-Duration", video.Length.TotalSeconds.ToString());

      // For range requests, we need to transcode the whole file up to the requested point
      var range = Request.Headers.Range.FirstOrDefault()?.Split('=').Last().Split('-');
      var startByte = range != null ? long.Parse(range[0]) : 0;

      var process = new Process
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "ffmpeg",
          Arguments = $"-i \"{videoPath}\" -c:v libx264 -preset ultrafast -c:a aac -f mp4 -movflags frag_keyframe+empty_moov pipe:1",
          RedirectStandardOutput = true,
          UseShellExecute = false,
          CreateNoWindow = true
        }
      };

      try
      {
        process.Start();
        var stream = process.StandardOutput.BaseStream;

        if (startByte > 0)
        {
          Response.StatusCode = 206;
          // Skip to the requested position
          await stream.ReadAsync(new byte[startByte], 0, (int)startByte);
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