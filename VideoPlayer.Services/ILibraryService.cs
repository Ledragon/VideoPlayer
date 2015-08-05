using System.Collections.Generic;
using System.Threading.Tasks;
using Classes;

namespace VideoPlayer.Services
{
    public interface ILibraryService
    {
        ObjectsWrapper GetObjectsFromFile();
        void Save();
        Task SaveAsync();
        void Clean();
        IEnumerable<Video> Update();
        Task<IEnumerable<Video>> UpdateAsync();
        Task CleanAsync();
    }
}