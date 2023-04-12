using LeDragon.Log.Standard;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.UnitOfWork
{
    public class TagVideoUnitOfWork : ITagVideoUnitOfWork
    {
        private readonly IVideoRepository _videoRepository;
        private readonly ITagsRepository _tagRepository;
        private readonly ITagVideoRepository _tagVideoRepository;
        private readonly ILogger _logger;

        public TagVideoUnitOfWork(IVideoRepository videoRepository, ITagsRepository tagRepository, ITagVideoRepository tagVideoRepository)
        {
            this._videoRepository = videoRepository;
            this._tagRepository = tagRepository;
            this._tagVideoRepository = tagVideoRepository;
            this._logger = this.Logger();
        }

        public async Task<Video> UpdateVideo(Video video)
        {
            try
            {
                if (video.Tags == null)
                {
                    video.Tags = new List<Tag>();
                }
                var newTags = video.Tags.Where(t => t.Id == 0).ToList();
                if (newTags.Any())
                {
                    newTags = this._tagRepository.Add(newTags);
                    video.Tags = video.Tags.Where(t => t.Id != 0).Concat(newTags).ToList();
                }
                var existing = (await this._tagVideoRepository.GetAsync()).Where(tv => tv.VideoId == video.Id);
                var toRemove = existing.Where(tv => !video.Tags.Any(t => t.Id == tv.TagId));
                video.TagVideos = existing.Except(toRemove).ToList();
                var result = this._videoRepository.Update(video);
                if (toRemove.Any())
                {
                    await this._tagVideoRepository.RemoveAsync(toRemove);
                }
                result.TagVideos.Clear();
                result.Tags.Clear();
                return result;
            }
            catch (Exception e)
            {
                this._logger.Error(e);
                throw;
            }
        }
    }
}