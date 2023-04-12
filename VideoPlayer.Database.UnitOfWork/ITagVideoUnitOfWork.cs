using VideoPlayer.Entities;

namespace VideoPlayer.Database.UnitOfWork
{
    public interface ITagVideoUnitOfWork
    {
        Task<Video> UpdateVideo(Video video);
    }
}