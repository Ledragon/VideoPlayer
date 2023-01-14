using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Database.Repository;
using VideoPlayer.Entities;

namespace VideoPlayer.WebAPI.Controllers
{
    [ApiController]
    [Route("api/videos")]
    public class VideosController : ControllerBase
    {
        private readonly ILogger<VideosController> _logger;
        private readonly IVideoRepository _videoRepository;

        public VideosController(ILogger<VideosController> logger, IVideoRepository videoRepository)
        {
            this._logger = logger;
            this._videoRepository = videoRepository;
        }

        [HttpGet]
        public List<Video> GetVideos()
        {
            var filePath = @"/home/hugues/Documents/Code/files/Library.xml";
            var videos = this._videoRepository.Load(filePath).Videos;
            return videos;
            //return new JsonResult(new List<Video>
            //{
            //    new Video{ Title="Harry Potter"},
            //    new Video{ Title = "Lord of the rings"},
            //    new Video{ Title  ="Dead Sushi"},
            //});
        }

        [HttpGet("/api/videos/metadata")]
        public Dictionary<String, VideoMetaData> GetMetaData()
        {
            var filePath = @"/home/hugues/Documents/Code/files/Library.xml";
            var videos = this._videoRepository.Load(filePath).Videos;
            return videos.ToDictionary(d => d.FileName, d => new VideoMetaData { Codec = "", HasContactSheet = System.IO.File.Exists(d.FileName + ".png") });
        }

        [HttpPost("/api/videos/previews")]
        public void GeneratePreviews([FromRoute] String path, [FromRoute] Int32 count)
        {

        }
    }
}
