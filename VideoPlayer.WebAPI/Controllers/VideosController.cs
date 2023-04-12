using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Database.UnitOfWork;
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
        private readonly ITagVideoUnitOfWork _tagVideoUnitOfWork;

        public VideosController(ILogger<VideosController> logger, IVideoRepository videoRepository, ITagVideoUnitOfWork tagVideoUnitOfWork)
        {
            this._logger = logger;
            this._videoRepository = videoRepository;
            this._tagVideoUnitOfWork = tagVideoUnitOfWork;
        }

        [HttpGet]
        public List<Video> GetVideos()
        {
            var videos = this._videoRepository.Get();
            return videos;
        }

        [HttpPut]
        public async Task<Video> Update(Video video)
        {
            return await this._tagVideoUnitOfWork.UpdateVideo(video);
        }

        [HttpGet("/api/videos/metadata")]
        public Dictionary<String, VideoMetaData> GetMetaData()
        {
            var videos = this._videoRepository.Get();
            return videos.ToDictionary(d => d.FileName, d => new VideoMetaData { Codec = "", HasContactSheet = System.IO.File.Exists(d.FileName + ".png") });
        }
    }
}
