using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Streaming.Attributes;
using VideoPlayer.Streaming.Models;

namespace VideoPlayer.Streaming.Controllers;

[ApiController]
[Route("api/scenes")]
public class SceneController : ControllerBase
{
    private readonly IStreamManager _streamManager;
    private readonly ILogger<SceneController> _logger;

    public SceneController(
        IStreamManager streamManager,
        ILogger<SceneController> logger)
    {
        _streamManager = streamManager;
        _logger = logger;
    }

    [HttpGet("{id}/stream/mkv")]
    [SceneContext]
    public Task StreamMkv() => 
        StreamTranscode(StreamFormat.Mkv);

    [HttpGet("{id}/stream/hls")]
    [SceneContext]
    public Task StreamHls() => 
        StreamManifest(StreamFormat.Hls, "HLS");

    [HttpGet("{id}/stream/dash")]
    [SceneContext]
    public Task StreamDash() => 
        StreamManifest(StreamFormat.Dash, "DASH");

    private async Task StreamTranscode(StreamFormat streamType)
    {
        var scene = HttpContext.GetScene();

        if (!_streamManager.IsTranscodingEnabled)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await HttpContext.Response.WriteAsync("Live transcoding disabled");
            return;
        }

        var primaryFile = scene.Files.GetPrimary();
        if (primaryFile == null)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        var startTime = HttpContext.Request.Query["start"].ToString();
        _ = double.TryParse(startTime, out var ss);
        var resolution = HttpContext.Request.Query["resolution"].ToString();

        var options = new TranscodeOptions
        {
            StreamType = streamType,
            VideoFile = primaryFile,
            Resolution = resolution,
            StartTime = ss
        };

        _logger.LogDebug("[transcode] streaming scene {SceneId} as {MimeType}", 
            scene.Id, streamType.MimeType);

        await _streamManager.ServeTranscodeAsync(HttpContext, options);
    }

    private async Task StreamManifest(StreamFormat streamType, string logName)
    {
        var scene = HttpContext.GetScene();

        if (!_streamManager.IsTranscodingEnabled)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await HttpContext.Response.WriteAsync("Live transcoding disabled");
            return;
        }

        var primaryFile = scene.Files.GetPrimary();
        if (primaryFile == null)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        // Additional manifest-specific logic here
    }
}