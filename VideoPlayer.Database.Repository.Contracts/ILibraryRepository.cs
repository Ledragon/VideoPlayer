using System;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.Contracts
{
    public interface ILibraryRepository
    {
        void Save(String filePath, ObjectsWrapper wrapper);
        ObjectsWrapper Load(String filePath);
    }
}