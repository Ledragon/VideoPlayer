using VideoPlayer.Database.Repository.Contracts;
using Directory = VideoPlayer.Entities.Directory;

namespace VideoPlayer.Database.Repository.SQLite
{
    public class SqliteDirectoryRepository : IDirectoryRepository
    {
        private VideoPlayerContext _context;

        public SqliteDirectoryRepository(VideoPlayerContext context)
        {
            this._context = context;
        }

        public Directory Add(Directory directory)
        {
            var entity = this._context.Directories.Add(directory);
            this._context.SaveChanges();
            return entity.Entity;
        }

        public List<Directory> Get()
        {
            return this._context.Directories.ToList();
        }
    }
}
