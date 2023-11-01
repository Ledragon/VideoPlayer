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
        private readonly IContactSheetService _contactSheetService;
        private readonly IAutoCompleteLibraryService _autoCompleteLibraryService;

        public VideosController(ILogger<VideosController> logger,
            IVideoRepository videoRepository,
            ITagVideoUnitOfWork tagVideoUnitOfWork,
            IContactSheetService contactSheetService,
            IAutoCompleteLibraryService autoCompleteLibraryService)
        {
            this._logger = logger;
            this._videoRepository = videoRepository;
            this._tagVideoUnitOfWork = tagVideoUnitOfWork;
            this._contactSheetService = contactSheetService;
            this._autoCompleteLibraryService = autoCompleteLibraryService;
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
            return await this._contactSheetService.CreateContactSheet(toto.VideoId, toto.NRows, toto.NCols);
        }
        [HttpPut("autoComplete")]
        public async Task AutoComplete()
        {
            var p = new Progress<String>();
            p.ProgressChanged += this.P_ProgressChanged;
            await this._autoCompleteLibraryService.AutoCompleteLibrary(p);
        }

        private void P_ProgressChanged(Object? sender, String e)
        {
            this._logger.LogDebug(e);
        }

        [HttpGet("/api/videos/metadata")]
        public Dictionary<String, VideoMetaData> GetMetaData()
        {
            var videos = this._videoRepository.Get();
            return videos.ToDictionary(d => d.FileName, d => new VideoMetaData { Codec = "", HasContactSheet = System.IO.File.Exists(d.FileName + ".png") });
        }
    }
}
