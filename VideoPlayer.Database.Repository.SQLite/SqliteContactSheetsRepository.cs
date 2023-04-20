using Microsoft.EntityFrameworkCore;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.SQLite
{
    public class SqliteContactSheetsRepository : IContactSheetsRepository
    {
        private readonly VideoPlayerContext _context;

        public SqliteContactSheetsRepository(VideoPlayerContext context)
        {
            this._context = context;
        }

        public async Task<ContactSheet> GetForVideo(Int32 videoId)
        {
            return await this._context.ContactSheets.FirstOrDefaultAsync(c => c.VideoId == videoId);
        }

        public async Task<IDictionary<Int32, ContactSheet>> GetForVideos(List<Int32> videoIds)
        {
            return await this._context.ContactSheets.Where(c => videoIds.Contains(c.VideoId)).ToDictionaryAsync(c => c.VideoId);
        }

        public async Task<ContactSheet> Add(ContactSheet contactSheet)
        {
            var entity = await this._context.ContactSheets.AddAsync(contactSheet);
            await this._context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<ContactSheet> Update(ContactSheet contactSheet)
        {
            var entity = this._context.ContactSheets.Update(contactSheet);
            await this._context.SaveChangesAsync();
            return entity.Entity;
        }
    }
}
