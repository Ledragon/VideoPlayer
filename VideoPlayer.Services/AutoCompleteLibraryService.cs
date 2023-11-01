using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;
using VideoPlayer.Ffmpeg;
using VideoPlayer.Helpers;

namespace VideoPlayer.Services
{
    public class AutoCompleteLibraryService : IAutoCompleteLibraryService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IFfmpegThumbnailGenerator _ffmpegThumbnailGenerator;
        private readonly IContactSheetService _contactSheetService;

        public AutoCompleteLibraryService(IVideoRepository videoRepository,
            IFfmpegThumbnailGenerator ffmpegThumbnailGenerator,
            IContactSheetService contactSheetService)
        {
            this._videoRepository = videoRepository;
            this._ffmpegThumbnailGenerator = ffmpegThumbnailGenerator;
            this._contactSheetService = contactSheetService;
        }

        public async Task AutoCompleteLibrary(IProgress<String> progress)
        {
            var videosWithoutThumbnails = await GenerateMissingThumbnails(progress);
            await this._videoRepository.UpdateAsync(videosWithoutThumbnails);

            var videosWithoutCs = await this._videoRepository.GetVideosWithoutCsAsync();
            videosWithoutCs.ForEach(v => this._contactSheetService.CreateContactSheet(v.Id, 3, 4));
        }

        private async Task<List<Video>> GenerateMissingThumbnails(IProgress<String> progress)
        {
            var videosWithoutThumbnails = await this._videoRepository.GetVideosWithoutThumbnailsAsync();
            progress.Report($"{videosWithoutThumbnails.Count} videos without thumbnails. generating...");
            videosWithoutThumbnails
                .OrderBy(d => d.Title)
                .ToList()
                .ForEach(video =>
                {
                    progress.Report($"Generating thumbnail for '{video.Title}'");
                    var thumbs = this._ffmpegThumbnailGenerator.GenerateThumbnails(video.FileName, 1);
                    thumbs.ForEach(t =>
                    {
                        var converted = ImageBase64Converter.ToBase64(t);
                        video.Thumbnails.Add(new Thumbnail { Image = converted });
                    });
                });
            return videosWithoutThumbnails;
        }
    }
}
