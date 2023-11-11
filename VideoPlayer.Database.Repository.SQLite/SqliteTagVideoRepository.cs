using Microsoft.EntityFrameworkCore;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.SQLite
{
    public class SqliteTagVideoRepository : ITagVideoRepository
    {
        private readonly VideoPlayerContext _context;

        public SqliteTagVideoRepository(VideoPlayerContext context)
        {
            this._context = context;
        }
        public List<TagVideo> Get()
        {
            return this._context.Videos.SelectMany(v => v.TagVideos).ToList();
        }

        public async Task<List<TagVideo>> GetAsync()
        {
            return await this._context.Videos.SelectMany(v => v.TagVideos).ToListAsync();
        }

        public void Remove(IEnumerable<TagVideo> toRemove)
        {

            this._context.RemoveRange(toRemove);
            this._context.SaveChanges();
        }

        public async Task RemoveAsync(IEnumerable<TagVideo> toRemove)
        {
            this._context.RemoveRange(toRemove);
            await this._context.SaveChangesAsync();
        }

        public async Task AddAsync(IEnumerable<TagVideo> toAdd)
        {
            this._context.AddRange(toAdd);
            await this._context.SaveChangesAsync();
        }

    }
}
