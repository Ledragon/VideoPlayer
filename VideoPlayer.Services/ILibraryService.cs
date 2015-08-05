using System.Threading.Tasks;
using Classes;

namespace VideoPlayer.Services
{
    public interface ILibraryService
    {
        ObjectsWrapper GetObjectsFromFile();
        void Save();
        void Clean();
        void Update();
        Task UpdateAsync();
    }
}