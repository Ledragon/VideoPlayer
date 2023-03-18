using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Database.Repository.Contracts;
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

        public VideosController(ILogger<VideosController> logger, IVideoRepository videoRepository)
        {
            this._logger = logger;
            this._videoRepository = videoRepository;
        }

        [HttpGet]
        public List<Video> GetVideos()
        {
            var videos = this._videoRepository.Get();
            return videos;
        }

        [HttpPut]
        public Video Update(Video video)
        {
            return this._videoRepository.Update(video);
        }

        [HttpGet("/api/videos/metadata")]
        public Dictionary<String, VideoMetaData> GetMetaData()
        {
            var videos = this._videoRepository.Get();
            return videos.ToDictionary(d => d.FileName, d => new VideoMetaData { Codec = "", HasContactSheet = System.IO.File.Exists(d.FileName + ".png") });
        }

        //[HttpPut]
        //public Video UpdateVideo([FromBody] Video video)
        //{
        //    this._logger.LogDebug(video.ToString());
        //    return video;
        //}
    }
}
