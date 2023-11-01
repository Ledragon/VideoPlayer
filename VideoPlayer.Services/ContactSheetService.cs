using System;
using System.Threading.Tasks;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;
using VideoPlayer.Ffmpeg;
using VideoPlayer.Helpers;

namespace VideoPlayer.Services
{
    public class ContactSheetService : IContactSheetService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IContactSheetsRepository _contactSheetsRepository;
        private readonly IFfmpegThumbnailGenerator _thumbnailGenerator;

        public ContactSheetService(IVideoRepository videoRepository, IContactSheetsRepository contactSheetsRepository, IFfmpegThumbnailGenerator thumbnailGenerator)
        {
            this._videoRepository = videoRepository;
            this._contactSheetsRepository = contactSheetsRepository;
            this._thumbnailGenerator = thumbnailGenerator;
        }

        public async Task<ContactSheet> CreateContactSheet(Int32 videoId, Int32 nRows, Int32 nColumns)
        {
            var video = this._videoRepository.Get(videoId);
            var cs = this._thumbnailGenerator.GenerateContactSheet(video.FileName, nRows, nColumns);
            var b64 = ImageBase64Converter.ToBase64(cs);
            var contactSheet = await this._contactSheetsRepository.GetForVideo(videoId);
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
        }
    }
}
