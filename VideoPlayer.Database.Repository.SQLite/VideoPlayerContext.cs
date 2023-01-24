using Microsoft.EntityFrameworkCore;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.SQLite
{
    public class VideoPlayerContext : DbContext
    {
        private String _dbPath;

        public VideoPlayerContext():this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VideoPlayer", "Temp", "Library.db"))
        {

        }

        public VideoPlayerContext(String path)
        {
            this._dbPath = path;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"DataSource={this._dbPath}");
        }

        public DbSet<Video> Videos { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }
    }
}