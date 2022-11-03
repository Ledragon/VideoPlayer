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
        IEnumerable<Video> Update();
        Task<IEnumerable<Video>> UpdateAsync();
        Task CleanAsync();
        void AddPlaylist(Playlist playlist);
        List<Video> GetVideosByFilePath(IEnumerable<String> fileNames);
        Task<ObjectsWrapper> LoadAsync();
        void ToJson(IEnumerable<Video> videos);
        Video GetVideoByFilePath(String fileName);
    }
}