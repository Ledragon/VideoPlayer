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

        public Directory? Remove(Int32 id)
        {
           var directory = this._context.Directories.SingleOrDefault(d => d.Id == id);
            if (directory != null)
            {
                this._context.Directories.Remove(directory);
                this._context.SaveChanges();
            }
            return directory;
        }
    }
}
