using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.SQLite
{
    public class SqliteThumbnailsRepository : IThumbnailsRepository
    {
        private readonly VideoPlayerContext _context;

        public SqliteThumbnailsRepository(VideoPlayerContext context)
        {
            this._context = context;
        }

        public Thumbnail Add(Thumbnail thumbnail)
        {
            var entityEntry = this._context.Add(thumbnail);
            this._context.SaveChanges();
            return entityEntry.Entity;
        }
        
        public List<Thumbnail> Add(List<Thumbnail> thumbnails)
        {
            this._context.AddRange(thumbnails);
            this._context.SaveChanges();
            return thumbnails;
        }

        public Thumbnail Delete(Int32 id)
        {
            var toRemove = this._context.Thumbnails.SingleOrDefault(d => d.Id == id);
            if (toRemove != null)
            {
                this._context.Remove(toRemove);
                this._context.SaveChanges();
            }
            return toRemove;
        }

        public List<Thumbnail> Get()
        {
            return this._context.Thumbnails.ToList();
        }

        public Thumbnail Get(Int32 id)
        {
            return this._context.Thumbnails.SingleOrDefault(d => d.Id == id);
        }

        public List<Thumbnail> GetForVideo(Int32 videoId)
        {
            return this._context.Thumbnails.Where(t => t.VideoId == videoId).ToList();
        }
    }
}
