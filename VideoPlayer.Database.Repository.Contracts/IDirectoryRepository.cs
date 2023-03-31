using System.Collections.Generic;

namespace VideoPlayer.Database.Repository.Contracts
{
    public interface IDirectoryRepository
    {
        List<Entities.Directory> Get();
    }
}