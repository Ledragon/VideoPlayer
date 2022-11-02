using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Classes;
using VideoPlayer.Entities;

namespace VideoPlayer.Services
{
    public interface ILibraryService
    {
        ObjectsWrapper GetObjectsFromFile();
        void Save();
        Task SaveAsync();
        void Clean();
        IEnumerable<Entities.Video> Update();
        Task<IEnumerable<Entities.Video>> UpdateAsync();
        Task CleanAsync();
        void AddPlaylist(Playlist playlist);
        List<Entities.Video> GetVideosByFilePath(IEnumerable<String> fileNames);
        Task<ObjectsWrapper> LoadAsync();
        void ToJson(IEnumerable<Entities.Video> videos);
    }
}