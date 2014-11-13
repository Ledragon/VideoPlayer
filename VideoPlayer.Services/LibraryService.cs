using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Classes;
using Log;
using VideoPlayer.Common;
using VideoPlayer.Database.Repository;
using VideoPlayer.Helpers;
using Directory = Classes.Directory;

namespace VideoPlayer.Services
{
    public class LibraryService : ILibraryService
    {
        public ObjectsWrapper GetObjectsFromFile()
        {
            var repository = DependencyFactory.Resolve<IVideoRepository>();
            ObjectsWrapper objectsFromFile = repository.Load(FileSystemHelper.GetDefaultFileName());
            return objectsFromFile;
        }

        public void Save(ObjectsWrapper wrapper)
        {
            this.Save(FileSystemHelper.GetDefaultFileName(), wrapper);
        }

        public void Save(String filePath, ObjectsWrapper wrapper)
        {
            var repository = DependencyFactory.Resolve<IVideoRepository>();
            repository.Save(filePath, wrapper);
        }


        public void Clean(ObservableCollection<Directory> directoryCollection,
            ObservableCollection<Video> videoCollection)
        {
            this.BackupLibrary();
            this.Logger().Info("Cleaning files.");
            List<String> existingFiles =
                directoryCollection
                    .SelectMany(
                        d => DirectoryHelper.GetVideoFiles(d.DirectoryPath, d.IsIncludeSubdirectories))
                    .ToList();
            List<String> videosToRemove = videoCollection
                .Select(t => t.FileName)
                .Except(existingFiles)
                .ToList();
            foreach (String file in videosToRemove)
            {
                Video video = videoCollection.Single(v => v.FileName == file);
                videoCollection.Remove(video);
                this.Logger().DebugFormat("File '{0}' removed.", video.FileName);
            }
            this.Logger().InfoFormat("'{0}' files removed.", videosToRemove.Count);
        }

        private void BackupLibrary()
        {
            this.BackupLibrary(FileSystemHelper.GetDefaultFileName());
        }

        private void BackupLibrary(String filePath)
        {
            this.Logger().Debug("Automatic backup of library.");
            String fileName = Path.GetFileNameWithoutExtension(filePath);
            String directory = Path.GetDirectoryName(filePath);
            String destionationFileName = fileName + "(0)";
            String destinationPath = Path.Combine(directory, destionationFileName + ".xml");
            Int32 i = 1;
            while (File.Exists(destinationPath))
            {
                destinationPath = Path.Combine(directory, String.Format("{0}({1}).xml", fileName, i));
                i++;
            }
            File.Copy(filePath, destinationPath);
            this.Logger().DebugFormat("Library backed up to '{0}'.", destinationPath);
        }
    }
}