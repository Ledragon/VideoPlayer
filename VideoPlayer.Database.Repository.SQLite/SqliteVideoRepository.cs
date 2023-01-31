using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.SQLite
{
    public class SqliteVideoRepository : IVideoRepository
    {
        private readonly VideoPlayerContext _context;

        public SqliteVideoRepository(VideoPlayerContext context)
        {
            this._context = context;
        }

        public Video Add(Video video)
        {
            var entity = this._context.Videos.Add(video);
            return entity.Entity;
        }

        public List<Video> Add(List<Video> videos)
        {
            this._context.Videos.AddRange(videos);
            return videos;
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

        public Video Update(Video video)
        {
            var entity = this._context.Update(video);
            return entity.Entity;
        }

        public List<Video> Update(List<Video> video)
        {
            this._context.UpdateRange(video);
            return video;
        }
    }
}