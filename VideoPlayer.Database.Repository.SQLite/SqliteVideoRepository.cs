using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.SQLite
{
    public class SqliteVideoRepository : IVideoRepository
    {
        public Video Add(Video video)
        {
            throw new NotImplementedException();
        }

        public List<Video> Add(List<Video> videos)
        {
            throw new NotImplementedException();
        }

        public List<Video> Get()
        {
            throw new NotImplementedException();
        }

        public Video Get(Int32 id)
        {
            throw new NotImplementedException();
        }

        public Video Get(String filePath)
        {
            throw new NotImplementedException();
        }

        public Video Update(Video video)
        {
            throw new NotImplementedException();
        }

        public List<Video> Update(List<Video> video)
        {
            throw new NotImplementedException();
        }
    }
}