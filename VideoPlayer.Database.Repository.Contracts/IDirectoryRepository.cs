using System.Collections.Generic;

namespace VideoPlayer.Database.Repository.Contracts
{
    public interface IDirectoryRepository
    {
        Entities.Directory Add(Entities.Directory directory);
        List<Entities.Directory> Get();
    }
}