using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Database.Repository;
using VideoPlayer.Entities;
using VideoPlayer.Services;

namespace VideoPlayer.WebAPI.Controllers
{
    [ApiController]
    [Route("api/videos")]
    public class VideosController : ControllerBase
    {
        private readonly ILogger<VideosController> _logger;
        private readonly IVideoRepository _videoRepository;
        private readonly IPathService _pathService;

        public VideosController(ILogger<VideosController> logger, 
            IVideoRepository videoRepository,
            IPathService pathService)
        {
            this._logger = logger;
            this._videoRepository = videoRepository;
            this._pathService = pathService;
        }

        [HttpGet]
        public List<Video> GetVideos()
        {
            var filePath = this._pathService.GetLibraryFile();
            var videos = this._videoRepository.Load(filePath).Videos;
            return videos;
        }

        [HttpGet("/api/videos/metadata")]
        public Dictionary<String, VideoMetaData> GetMetaData()
        {
            var filePath = this._pathService.GetLibraryFile();
            var videos = this._videoRepository.Load(filePath).Videos;
            return videos.ToDictionary(d => d.FileName, d => new VideoMetaData { Codec = "", HasContactSheet = System.IO.File.Exists(d.FileName + ".png") });
        }
    }
}
