using System;
using System.Collections.ObjectModel;
using Classes;

namespace VideoPlayer.Services
{
    public interface ILibraryService
    {
        ObjectsWrapper GetObjectsFromFile();
        void Save();
        //void Save(ObjectsWrapper wrapper);
        //void Save(String filePath, ObjectsWrapper wrapper);
        void Clean();
        //void Clean(ObservableCollection<Directory> directoryCollection, ObservableCollection<Video> videoCollection);
        void Update();
    }
}