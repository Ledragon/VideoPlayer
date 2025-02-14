using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;
using VideoPlayer.Ffmpeg;
using VideoPlayer.Helpers;

namespace VideoPlayer.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ThumbnailsController : ControllerBase
    {
        private readonly IFfmpegThumbnailGenerator _thumbnailGenerator;
        private readonly IThumbnailsRepository _thumbnailsRepository;

        public ThumbnailsController(IFfmpegThumbnailGenerator thumbnailGenerator, IThumbnailsRepository thumbnailsRepository)
        {
            this._thumbnailGenerator = thumbnailGenerator;
            this._thumbnailsRepository = thumbnailsRepository;
        }

        [HttpGet]
        public List<Thumbnail> Get()
        {
            return this._thumbnailsRepository.Get();
        }

        [HttpGet("forVideo")]
        public List<Thumbnail> Get([FromQuery] Int32 videoId)
        {
            return this._thumbnailsRepository.GetForVideo(videoId);
        }

        [HttpGet("generate")]
        public List<String> Generate([FromQuery] String filePath, [FromQuery] Int32 count, [FromQuery] Int32 width)
        {
            var files = this._thumbnailGenerator.GenerateThumbnails(filePath, count, width);
            return files
                .Select(f => ImageBase64Converter.ToBase64(f))
                .ToList();
        }

        [HttpPost]
        public List<Thumbnail> Add([FromBody] List<Thumbnail> thumbnails)
        {
            return this._thumbnailsRepository.Add(thumbnails);
        }

        [HttpDelete]
        public Thumbnail Delete([FromQuery] Int32 id)
        {
            return this._thumbnailsRepository.Delete(id);
        }
    }
}
