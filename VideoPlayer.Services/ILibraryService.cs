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
        void Update();
        Task UpdateAsync();
        Task CleanAsync();
    }
}