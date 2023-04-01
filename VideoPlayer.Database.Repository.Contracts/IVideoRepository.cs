using System;
using System.Collections.Generic;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.Contracts
{
    public interface IVideoRepository
    {
        List<Video> Get();
        Video Get(Int32 id);
        Video Get(String filePath);
        Video Add(Video video);
        List<Video> Add(List<Video> videos);
        Video Update(Video video);
        List<Video> Update(List<Video> video);
        void Delete(List<Video> notFound);
    }
}
