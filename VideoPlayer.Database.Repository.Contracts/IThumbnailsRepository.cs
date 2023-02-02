using System;
using System.Collections.Generic;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.Contracts
{
    public interface IThumbnailsRepository
    {
        List<Thumbnail> Get();
        Thumbnail Get(Int32 id);
        List<Thumbnail> GetForVideo(Int32 videoId);
        Thumbnail Add(Thumbnail thumbnail);
        List<Thumbnail> Add(List<Thumbnail> thumbnails);
        Thumbnail Delete(Int32 id);
    }
}
