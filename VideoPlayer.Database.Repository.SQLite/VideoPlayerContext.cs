using Microsoft.EntityFrameworkCore;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.SQLite
{
    public class VideoPlayerContext : DbContext
    {
        public DbSet<Video> Videos { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Thumbnail> Thumbnails{ get; set; }
    }
}