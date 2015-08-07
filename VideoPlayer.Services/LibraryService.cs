﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        // Singleton-like
        private static ObjectsWrapper _objectsWrapper;

        public ObjectsWrapper GetObjectsFromFile()
        {
            if (_objectsWrapper == null)
            {
                var repository = DependencyFactory.Resolve<IVideoRepository>();
                _objectsWrapper = repository.Load(FileSystemHelper.GetDefaultFileName());
            }
            return _objectsWrapper;
        }

        public void Save()
        {
            this.Save(this.GetObjectsFromFile());
        }

        public async Task SaveAsync()
        {
            await Task.Factory.StartNew(this.Save);
        }

        public void Clean()
        {
            this.Clean(this.GetObjectsFromFile().Directories, this.GetObjectsFromFile().Videos);
        }

        public async Task CleanAsync()
        {
            await Task.Factory.StartNew(this.Clean);
        }

        public IEnumerable<Video> Update()
        {
            IEnumerable<Video> result = new List<Video>();
            try
            {
                this.Logger().DebugFormat("Updating library.");
                var wrapper = this.GetObjectsFromFile();
                var videoList = wrapper.Videos;
                var categories =
                    videoList.Where(v => v.Category != null).Select(v => v.Category.ToLower()).OrderBy(c => c).ToList();
                foreach (var directory in wrapper.Directories)
                {
                    var files =
                        DirectoryHelper.GetVideoFiles(directory.DirectoryPath, directory.IsIncludeSubdirectories)
                            .Where(videoFile => videoList.All(s => s.FileName != videoFile))
                            .ToList();
                    this.Logger().DebugFormat("'{0}' new files found.", files.Count());
                    foreach (var videoFile in files)
                    {
                        var newVideo = new Video(videoFile);
                        var firstCategory = categories.FirstOrDefault(c => newVideo.Title.ToLower().Contains(c));
                        if (firstCategory != null)
                        {
                            newVideo.Category = firstCategory;
                        }
                        newVideo.DateAdded = DateTime.Now;
                        videoList.Add(newVideo);
                        this.Logger().DebugFormat("File '{0}' added.", newVideo.FileName);
                    }
                }
                this.Save();
                result = videoList;
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
                throw;
            }
            return result;
        }

        public async Task<IEnumerable<Video>>  UpdateAsync()
        {
            return await Task.Factory.StartNew(() => this.Update());
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
            List<Video> videoCollection)
        {
            this.BackupLibrary();
            this.Logger().Info("Cleaning files.");
            var existingFiles =
                directoryCollection
                    .SelectMany(
                        d => DirectoryHelper.GetVideoFiles(d.DirectoryPath, d.IsIncludeSubdirectories))
                    .ToList();
            var videosToRemove = videoCollection
                .Select(t => t.FileName)
                .Except(existingFiles)
                .ToList();
            foreach (var file in videosToRemove)
            {
                var video = videoCollection.Single(v => v.FileName == file);
                videoCollection.Remove(video);
                this.Logger().DebugFormat("File '{0}' removed.", video.FileName);
            }
            this.Logger().InfoFormat("'{0}' files removed.", videosToRemove.Count);
        }

        public void AddPlaylist(Playlist playlist)
        {
            this.GetObjectsFromFile().PlayLists.Add(playlist);
        }

        private void BackupLibrary()
        {
            this.BackupLibrary(FileSystemHelper.GetDefaultFileName());
        }

        private void BackupLibrary(String filePath)
        {
            this.Logger().Debug("Automatic backup of library.");
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var directory = Path.GetDirectoryName(filePath);
            var destionationFileName = fileName + "(0)";
            var destinationPath = Path.Combine(directory, destionationFileName + ".xml");
            var i = 1;
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