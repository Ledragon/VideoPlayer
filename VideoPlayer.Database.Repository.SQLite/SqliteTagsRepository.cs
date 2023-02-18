using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.SQLite
{
    public class SqliteTagsRepository : ITagsRepository
    {
        private readonly VideoPlayerContext _context;

        public SqliteTagsRepository(VideoPlayerContext context)
        {
            this._context = context;
        }
            
        public Tag Add(Tag tag)
        {
            var entityEntry = this._context.Add(tag);
            this._context.SaveChanges();
            return entityEntry.Entity;
        }

        public List<Tag> Add(List<Tag> tags)
        {
            this._context.AddRange(tags);
            this._context.SaveChanges();
            return tags;
        }

        public Tag Delete(Int32 id)
        {
            var toRemove = this._context.Tags.SingleOrDefault(d => d.Id == id);
            if (toRemove != null)
            {
                this._context.Remove(toRemove);
                this._context.SaveChanges();
            }
            return toRemove;
        }

        public List<Tag> Get()
        {
            return this._context.Tags.ToList();
        }

        public Tag Get(Int32 id)
        {
            return this._context.Tags.SingleOrDefault(d => d.Id == id);
        }

        public List<Tag> GetForVideo(Int32 videoId)
        {
            throw new NotImplementedException();
        }
    }
}
