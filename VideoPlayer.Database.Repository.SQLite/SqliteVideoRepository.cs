using LeDragon.Log.Standard;
using Microsoft.EntityFrameworkCore;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.SQLite
{
    public class SqliteVideoRepository : IVideoRepository
    {
        private readonly VideoPlayerContext _context;
        private readonly ILogger _logger;

        public SqliteVideoRepository(VideoPlayerContext context)
        {
            this._context = context;
            this._logger = this.Logger();
        }

        public Video Add(Video video)
        {
            var entity = this._context.Videos.Add(video);
            this._context.SaveChanges();
            return entity.Entity;
        }

        public List<Video> Add(List<Video> videos)
        {
            this._context.Videos.AddRange(videos);
            this._context.SaveChanges();
            return videos;
        }

        public void Delete(List<Video> notFound)
        {
            this._context.Videos.RemoveRange(notFound);
            this._context.SaveChanges();
        }

        public List<Video> Get()
        {
            return this._context.Videos.ToList();
        }

        public Video Get(Int32 id)
        {
            return this._context.Videos.SingleOrDefault(v => v.Id == id);
        }

        public Video Get(String filePath)
        {
            return this._context.Videos.SingleOrDefault(v => v.FileName == filePath);
        }

        public async Task<List<Video>> GetVideosWithoutThumbnailsAsync()
        {
            return await this._context.Videos.Where(v => v.Thumbnails == null || !v.Thumbnails.Any())
                .Include(v => v.Thumbnails)
                .ToListAsync();
        }
        public async Task<List<Video>> GetVideosWithoutCsAsync()
        {
            return await this._context.Videos.Where(v => v.ContactSheet == null || String.IsNullOrEmpty(v.ContactSheet.Image)).ToListAsync();
        }

        public Video Update(Video video)
        {
            try
            {
                var entity = this._context.Update(video);
                this._context.SaveChanges();
                return entity.Entity;
            }
            catch (Exception e)
            {
                this._logger.Error(e);
                throw;
            }
        }

        public List<Video> Update(List<Video> video)
        {
            this._context.UpdateRange(video);
            this._context.SaveChanges();
            return video;
        }

        public async Task<List<Video>> UpdateAsync(List<Video> video)
        {
            this._context.UpdateRange(video);
            await this._context.SaveChangesAsync();
            return video;
        }
    }
}