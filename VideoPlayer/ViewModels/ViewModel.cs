using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Classes;
using Controllers;
using log4net;

namespace VideoPlayer.ViewModels
{
    internal class ViewModel
    {
        private readonly ObservableCollection<Video> _videos;
        private readonly ObservableCollection<Directory> _directories;
        private readonly Controller _controller;
        private Dispatcher _dispatcher;
        private readonly ObjectsWrapper _wrapper;
        private readonly ILog _logger;

        public ViewModel()
        {
            this._logger = LogManager.GetLogger(typeof(ViewModel));
            _videos = new ObservableCollection<Video>();
            _directories = new ObservableCollection<Directory>();
            _controller = new Controller();
            this._logger.Info("Decoding file.");
            this._wrapper = this._controller.GetObjectsFromFile();
            if (this._wrapper != null)
            {
                this._videos = this._wrapper.Videos;
                this._directories = this._wrapper.Directories;
            }
            else
            {
                this._wrapper = new ObjectsWrapper
                {
                    Videos = this._videos,
                    Directories = this._directories
                };

            }
            this._logger.Info("File decoded.");
        }

        public void Save()
        {
            this._controller.Save(this._wrapper);
        }

        public ObservableCollection<Video> VideoCollection
        {
            get
            {
                return this._videos;
            }
        }

        public ObservableCollection<Directory> DirectoryCollection
        {
            get
            {
                return this._directories;
            }
        }

        public void Clean()
        {
            this._logger.Info("Cleaning files");
            var existingFiles = this._directories.SelectMany(directory => this._controller.GetVideoFiles(directory)).ToList();
            var videosToRemove = this._videos.Select(t => t.FileName).Except(existingFiles).ToList();
            foreach (var file in videosToRemove)
            {
                foreach (var video in this._videos)
                {
                    if (video.FileName == file)
                    {
                        this._videos.Remove(video);
                        this._logger.InfoFormat("File {0} removed.", video.FileName);
                        break;
                    }
                }
            }
            this._logger.Info("Files cleaned.");
        }

        public void Load(Dispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
            BackgroundWorker backgroundWorkerLoad = new BackgroundWorker();
            backgroundWorkerLoad.DoWork += this.backgroundWorkerLoad_DoWork;
            backgroundWorkerLoad.RunWorkerCompleted += backgroundWorkerLoad_RunWorkerCompleted;
            backgroundWorkerLoad.RunWorkerAsync();
        }

        private void backgroundWorkerLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //TODO pourri
            MessageBox.Show("Finished loading");
            //this._uiCurrentOperationStatusBarItem.Content = "Ready";
        }

        private void backgroundWorkerLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            Action<Video> addMethod = video => this._videos.Add(video);
            Video[] tmpList = this._videos.ToArray();
            foreach (var directory in this._directories)
            {
                var files = this._controller.GetVideoFiles(directory);
                foreach (String videoFile in files)
                {
                    if (tmpList.All(s => s.FileName != videoFile))
                    {
                        Video newVideo = new Video(videoFile);
                        // cross-thread
                        this._dispatcher.BeginInvoke(addMethod, newVideo);
                        newVideo.DateAdded = DateTime.Now;
                        this._logger.InfoFormat("File {0} added.", newVideo.FileName);
                    }
                }
            }
        }

    }
}
