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
            var filePath = @"E:\Hugues Stefanski\Documents\Development\GitHub\VideoPlayer\VideoPlayer\bin\Debug\Files\Library.xml";
            return this._videoRepository.Load(filePath).Videos;
            //return new JsonResult(new List<Video>
            //{
            //    new Video{ Title="Harry Potter"},
            //    new Video{ Title = "Lord of the rings"},
            //    new Video{ Title  ="Dead Sushi"},
            //});
        }
    }
}
