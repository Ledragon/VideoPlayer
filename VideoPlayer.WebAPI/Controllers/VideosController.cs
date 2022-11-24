using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Entities;

namespace VideoPlayer.WebAPI.Controllers
{
    [ApiController]
    [Route("api/videos")]
    public class VideosController : ControllerBase
    {
        private readonly ILogger<VideosController> _logger;

        public VideosController(ILogger<VideosController> logger)
        {
            this._logger = logger;
        }

        [HttpGet]
        public JsonResult GetVideos()
        {
            return new JsonResult(new List<Video>
            {
                new Video{ Title="Harry Potter"},
                new Video{ Title = "Lord of the rings"},
                new Video{ Title  ="Dead Sushi"},
            });
        }
    }
}
