using System.Collections.Generic;
using System.Threading.Tasks;
using VideoPlayer.Entities;

namespace VideoPlayer.Database.Repository.Contracts
{
    public interface ITagVideoRepository
    {
        List<TagVideo> Get();
        Task<List<TagVideo>> GetAsync();
    }
}