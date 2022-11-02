using System;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository
{
    public interface IVideoRepository
    {
        void Save(String filePath, ObjectsWrapper wrapper);
        ObjectsWrapper Load(String filePath);
    }
}