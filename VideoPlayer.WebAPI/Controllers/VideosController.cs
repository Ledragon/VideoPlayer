using Microsoft.AspNetCore.Mvc;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Database.UnitOfWork;
using VideoPlayer.Entities;
using VideoPlayer.Ffmpeg;
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
        private readonly IFfmpegThumbnailGenerator _ffmpegThumbnailGenerator;
        private readonly IContactSheetsRepository _contactSheetsRepository;

        public VideosController(ILogger<VideosController> logger, IVideoRepository videoRepository, ITagVideoUnitOfWork tagVideoUnitOfWork, IFfmpegThumbnailGenerator ffmpegThumbnailGenerator,
            IContactSheetsRepository contactSheetsRepository)
        {
            this._logger = logger;
            this._videoRepository = videoRepository;
            this._tagVideoUnitOfWork = tagVideoUnitOfWork;
            this._ffmpegThumbnailGenerator = ffmpegThumbnailGenerator;
            this._contactSheetsRepository = contactSheetsRepository;
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

        [HttpPut("contactSheet")]
        public async Task<ContactSheet> CreateContactSheet(Params toto)
        {
            var video = this._videoRepository.Get(toto.VideoId);
            var cs = this._ffmpegThumbnailGenerator.GenerateContactSheet(video.FileName, toto.NRows, toto.NCols);
            var b64 = ToBase64(cs);
            var contactSheet = await this._contactSheetsRepository.GetForVideo(toto.VideoId);
            if (contactSheet != null)
            {
                contactSheet.Image = b64;
                contactSheet = await this._contactSheetsRepository.Update(contactSheet);
            }
            else
            {
                contactSheet = new ContactSheet { Image = b64, Video = video, VideoId = video.Id }; ;
                contactSheet = await this._contactSheetsRepository.Add(contactSheet);
            }
            return contactSheet;
            //video.ContactSheet = b64;
            //return this._videoRepository.Update(video);
        }

        [HttpGet("/api/videos/metadata")]
        public Dictionary<String, VideoMetaData> GetMetaData()
        {
            var videos = this._videoRepository.Get();
            return videos.ToDictionary(d => d.FileName, d => new VideoMetaData { Codec = "", HasContactSheet = System.IO.File.Exists(d.FileName + ".png") });
        }
        private static String ToBase64(String t)
        {
            if (System.IO.File.Exists(t))
            {
                var bytes = System.IO.File.ReadAllBytes(t);
                var converted = Convert.ToBase64String(bytes);
                return converted;
            }
            else
            {
                return String.Empty;
            }
        }
    }

    public class Params
    {
        public Int32 VideoId { get; set; }
        public Int32 NRows { get; set; }
        public Int32 NCols { get; set; }
    }
}
