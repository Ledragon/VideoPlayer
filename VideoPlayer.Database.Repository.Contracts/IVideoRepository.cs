using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        Task<List<Video>> UpdateAsync(List<Video> video);
        void Delete(List<Video> notFound);
        Task<List<Video>> GetVideosWithoutThumbnailsAsync();
        Task<List<Video>> GetVideosWithoutCsAsync();

    }
}
