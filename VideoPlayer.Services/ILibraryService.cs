using System;
using System.Collections.ObjectModel;
using Classes;

namespace VideoPlayer.Services
{
    public interface ILibraryService
    {
        ObjectsWrapper GetObjectsFromFile();
        void Save(ObjectsWrapper wrapper);
        void Save(String filePath, ObjectsWrapper wrapper);
        void Clean(ObservableCollection<Directory> directoryCollection, ObservableCollection<Video> videoCollection);
    }
}