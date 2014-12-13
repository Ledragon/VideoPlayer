using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Classes;
using Log;
using VideoPlayer.Common;
using VideoPlayer.Helpers;
using VideoPlayer.Services;

namespace VideoPlayer.ViewModels
{
    internal class ViewModel
    {
        //private readonly Controller _controller;
        private Dispatcher _dispatcher;
        //private ObjectsWrapper _wrapper;

        public ViewModel()
        {
            //this._controller = new Controller();
            this.LoadFile();
        }

        public ObservableCollection<Video> VideoCollection { get; private set; }

        public ObservableCollection<Directory> DirectoryCollection { get; private set; }

        private void LoadFile()
        {
            this.Logger().Debug("Decoding file.");
            var libraryService = DependencyFactory.Resolve<ILibraryService>();
            var wrapper = libraryService.GetObjectsFromFile();
            this.VideoCollection = wrapper.Videos;
            this.DirectoryCollection = wrapper.Directories;
            this.Logger().Debug("File decoded.");
        }

        public void Save()
        {
            var libraryService = DependencyFactory.Resolve<ILibraryService>();
            libraryService.Save();
        }

        //public void Clean()
        //{
        //    var libraryService = DependencyFactory.Resolve<ILibraryService>();
        //    libraryService.Clean(this.DirectoryCollection, this.VideoCollection);
        //    //this.Logger().Info("Cleaning files");
        //    //List<string> existingFiles =
        //    //    this.DirectoryCollection.SelectMany(d=>DirectoryHelper.GetVideoFiles(d.DirectoryPath, d.IsIncludeSubdirectories)).ToList();
        //    //List<string> videosToRemove = this.VideoCollection.Select(t => t.FileName).Except(existingFiles).ToList();
        //    //foreach (string file in videosToRemove)
        //    //{
        //    //    foreach (Video video in this.VideoCollection)
        //    //    {
        //    //        if (video.FileName == file)
        //    //        {
        //    //            this.VideoCollection.Remove(video);
        //    //            this.Logger().InfoFormat("File {0} removed.", video.FileName);
        //    //            break;
        //    //        }
        //    //    }
        //    //}
        //    //this.Logger().Info("Files cleaned.");
        //}

        public void Load(Dispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
            var backgroundWorkerLoad = new BackgroundWorker();
            backgroundWorkerLoad.DoWork += this.backgroundWorkerLoad_DoWork;
            backgroundWorkerLoad.RunWorkerCompleted += this.backgroundWorkerLoad_RunWorkerCompleted;
            backgroundWorkerLoad.RunWorkerAsync();
        }

        private void backgroundWorkerLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //TODO pourri
            MessageBox.Show("Finished loading");
            //this._dispatcher.BeginInvoke(DispatcherPriority.Normal, UpdateStatus);
        }

        //private void UpdateStatus()
        //{
        //    this._uiCurrentOperationStatusBarItem.Content = "Ready";

        //}

        private void backgroundWorkerLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            Action<Video> addMethod = video => this.VideoCollection.Add(video);
            Video[] tmpList = this.VideoCollection.ToArray();
            List<string> categories = tmpList.Select(v => v.Category).OrderBy(c => c).ToList();
            foreach (Directory directory in this.DirectoryCollection)
            {
                List<string> files = DirectoryHelper.GetVideoFiles(directory.DirectoryPath, directory.IsIncludeSubdirectories);
                foreach (String videoFile in files)
                {
                    if (tmpList.All(s => s.FileName != videoFile))
                    {
                        var newVideo = new Video(videoFile);
                        string firstCategory = categories.FirstOrDefault(c => newVideo.Title.Contains(c));
                        if (firstCategory != null)
                        {
                            newVideo.Category = firstCategory;
                        }
                        // cross-thread
                        this._dispatcher.BeginInvoke(addMethod, newVideo);
                        newVideo.DateAdded = DateTime.Now;
                        this.Logger().DebugFormat("File '{0}' added.", newVideo.FileName);
                    }
                }
            }
        }
    }
}