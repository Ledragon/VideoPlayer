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

        public List<Directory> Get()
        {
            return this._context.Directories.ToList();
        }
    }
}
