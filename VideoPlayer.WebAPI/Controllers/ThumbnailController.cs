using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;
using VideoPlayer.Ffmpeg;

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

        //[HttpGet]
        //public List<String> Thumbnails([FromQuery] String filePath, [FromQuery] Int32 count)
        //{
        //    var files = this._thumbnailGenerator.GenerateThumbnails(filePath, count);
        //    return files
        //        .Select(f =>
        //        {
        //            var bytes = System.IO.File.ReadAllBytes(f);
        //            var converted = Convert.ToBase64String(bytes);
        //            return converted;
        //        })
        //        .ToList();
        //}
    }
}
