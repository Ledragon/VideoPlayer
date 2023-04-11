using System;
using System.Collections.Generic;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.Contracts
{
    public interface IDirectoryRepository
    {
        Directory Add(Directory directory);
        List<Directory> Get();
        Directory Remove(Int32 id);
    }
}