using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Classes;
using Log;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;

namespace VideosListModule.ViewModels
{
    public class VideosListViewModel : ViewModelBase, IVideosListViewModel
    {
        private Video _currentVideo;
        private Int32 _editIndex;
        private IEventAggregator _eventAggregator;
        private VideosCollectionView _filteredVideos;
        private Visibility _infoVisibility;
        private Boolean _isLoading;
        private ILibraryService _libraryService;

        public VideosListViewModel(ILibraryService libraryService, IVideosListView videosListView,
            IEventAggregator eventAggregator)
            : base(videosListView)
        {
            this.Init(libraryService, eventAggregator);
        }

        public ICommand LoadDataAsyncCommand { get; private set; }

        public Boolean IsLoading
        {
            get { return this._isLoading; }
            set
            {
                if (value == this._isLoading)
                {
                    return;
                }
                this._isLoading = value;
                this.OnPropertyChanged();
            }
        }

        public DelegateCommand AddVideoCommand { get; private set; }
        public DelegateCommand PlayOneCommand { get; private set; }
        public DelegateCommand PlayPlaylistCommand { get; private set; }
        public DelegateCommand NextCommand { get; private set; }
        public DelegateCommand PreviousCommand { get; private set; }

        public Visibility InfoVisibility
        {
            get { return this._infoVisibility; }
            set
            {
                if (value == this._infoVisibility)
                {
                    return;
                }
                this._infoVisibility = value;
                this.OnPropertyChanged();
            }
        }

        public VideosCollectionView FilteredVideos
        {
            get { return this._filteredVideos; }
            set
            {
                if (Equals(value, this._filteredVideos))
                {
                    return;
                }
                this._filteredVideos = value;

                this._eventAggregator.GetEvent<FilterChangedEvent>().Publish(this.FilteredVideos.Cast<Video>().Count());
                this.Raise();
                this.OnPropertyChanged();
            }
        }

        public Int32 EditIndex
        {
            get { return this._editIndex; }
            set
            {
                if (value == this._editIndex)
                {
                    return;
                }
                this._editIndex = value;
                this.OnPropertyChanged();
            }
        }

        public Video CurrentVideo
        {
            get { return this._currentVideo; }
            set
            {
                if (!Equals(value, this._currentVideo))
                {
                    this._currentVideo = value;
                    this._eventAggregator.GetEvent<VideoSelected>().Publish(value);
                    this.OnPropertyChanged();
                }
            }
        }

        public async Task Init()
        {
            this.IsLoading = true;
            var wrapper = await this._libraryService.LoadAsync();
            var videos = wrapper.Videos;
            this.UpdateVideoListView(videos);
            this.IsLoading = false;
        }

        private void Init(ILibraryService libraryService, IEventAggregator eventAggregator)
        {
            this._libraryService = libraryService;
            try
            {
                this._eventAggregator = eventAggregator;

                this._eventAggregator.GetEvent<SelectedCategoryChangedEvent>().Subscribe(this.FilterCategory);
                this._eventAggregator.GetEvent<NameFilterChangedEvent>().Subscribe(this.FilterName);
                this._eventAggregator.GetEvent<TagFilterChangedEvent>().Subscribe(this.FilterTag);
                this._eventAggregator.GetEvent<PlayAllEvent>().Subscribe(this.PlayAll);
                this._eventAggregator.GetEvent<SortingChangedEvent>().Subscribe(this.Sort);
                this._eventAggregator.GetEvent<LibraryUpdated>().Subscribe(this.UpdateVideoListView);
                this._eventAggregator.GetEvent<OnAddVideoRangeRequest>().Subscribe(this.AddRange);
                this.InfoVisibility = Visibility.Visible;

                this.AddVideoCommand = new DelegateCommand(this.Add, this.CanCommandsExecute);
                this.PlayPlaylistCommand = new DelegateCommand(this.PlayPlaylist, this.CanCommandsExecute);
                this.PlayOneCommand = new DelegateCommand(this.PlayOne, this.CanCommandsExecute);
                this.NextCommand = new DelegateCommand(() =>
                {
                    this.FilteredVideos.NextPage();
                    this.Raise();
                },
                    () => this.FilteredVideos != null && this.FilteredVideos.CanMoveToNextPage);

                this.PreviousCommand = new DelegateCommand(() =>
                {
                    this.FilteredVideos.PreviousPage();
                    this.Raise();
                },
                    () => this.FilteredVideos != null && this.FilteredVideos.CanMoveToPreviousPage);
                this.LoadDataAsyncCommand = new DelegateCommand(async () => await this.Init());
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
                throw;
            }
        }

        private void UpdateVideoListView(IEnumerable<Video> videos)
        {
            FilterParameters filter = null;
            if (this.FilteredVideos != null)
            {
                filter = this.FilteredVideos.GetFilter();
            }
            this.FilteredVideos = new VideosCollectionView(new ObservableCollection<Video>(videos),5);
            if (filter != null)
            {
                this.FilteredVideos.FilterTag(filter.Tags);
                this.FilteredVideos.FilterCategory(filter.Category);
                this.FilteredVideos.FilterName(filter.Name);
                this.FilteredVideos.Sort(filter.SortDescription);
            }
            this._eventAggregator.GetEvent<FilterChangedEvent>().Publish(this.FilteredVideos.Cast<Video>().Count());
        }

        private void Raise()
        {
            this.NextCommand.RaiseCanExecuteChanged();
            this.PreviousCommand.RaiseCanExecuteChanged();
        }

        private void FilterTag(List<String> tags)
        {
            this.FilteredVideos.FilterTag(tags);
            this.Raise();
        }

        private void FilterName(String obj)
        {
            this.FilteredVideos.FilterName(obj);
            this.Raise();
        }

        private void FilterCategory(String category)
        {
            this.FilteredVideos.FilterCategory(category);
            this.Raise();
        }

        private Boolean CanCommandsExecute()
        {
            return this.EditIndex == 0;
        }

        private void Sort(SortDescription obj)
        {
            this.FilteredVideos.Sort(obj);
        }

        private void PlayAll(Object obj)
        {
            this._eventAggregator.GetEvent<ClearPlaylistEvent>().Publish(null);
            var videoAddedEvent = this._eventAggregator.GetEvent<OnAddVideo>();
            foreach (var video in this.FilteredVideos.Cast<Video>())
            {
                videoAddedEvent.Publish(video);
            }
            this.PlayPlaylist();
        }

        private void AddRange(Object obj)
        {
            this._eventAggregator.GetEvent<OnAddVideoRange>().Publish(this.FilteredVideos.Cast<Video>());
        }

        private void PlayOne()
        {
            if (File.Exists(this.CurrentVideo.FileName))
            {
                this._eventAggregator.GetEvent<PlayOneEvent>().Publish(this.CurrentVideo);
            }
            else
            {
                MessageBox.Show("File not found.");
            }
        }

        private void PlayPlaylist()
        {
            this._eventAggregator.GetEvent<OnPlayPlaylistRequest>().Publish(null);
        }

        private void Add()
        {
            if (this.CurrentVideo != null && File.Exists(this.CurrentVideo.FileName))
            {
                this._eventAggregator.GetEvent<OnAddVideo>().Publish(this.CurrentVideo);
            }
        }
    }
}