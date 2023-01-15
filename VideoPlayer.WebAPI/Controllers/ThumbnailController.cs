using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Ffmpeg;

namespace VideoPlayer.WebAPI.Controllers
{
    [Route("api/thumbnails")]
    public class ThumbnailController : ControllerBase
    {
        private readonly IFfmpegThumbnailGenerator _thumbnailGenerator;

        public ThumbnailController(IFfmpegThumbnailGenerator thumbnailGenerator)
        {
            this._thumbnailGenerator = thumbnailGenerator;
        }

        [HttpGet("/api/thumbnails")]
        public List<String> Thumbnails([FromQuery] String filePath, [FromQuery] Int32 count)
        {
            var files = this._thumbnailGenerator.GenerateThumbnails(filePath, count);
            return files
                .Select(f =>
                {
                    var bytes = System.IO.File.ReadAllBytes(f);
                    var converted = Convert.ToBase64String(bytes);
                    return converted;
                })
                .ToList();
        }
    }
}
