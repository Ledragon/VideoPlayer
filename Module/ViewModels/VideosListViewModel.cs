using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Classes;
using Log;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;

namespace Module
{
    public class VideosListViewModel : ViewModelBase, IVideosListViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private Video _currentVideo;
        private ICollectionView _filteredVideos;
        private Visibility _infoVisibility;
        private ObservableCollection<Video> _videoCollection;

        public VideosListViewModel(ILibraryService libraryService, IVideosList videosList,
            IEventAggregator eventAggregator)
            : base(videosList)
        {
            this._eventAggregator = eventAggregator;
            this._eventAggregator.GetEvent<SelectedCategoryChangedEvent>().Subscribe(this.FilterCategory);
            this.VideoCollection = libraryService.GetObjectsFromFile().Videos;
            this.FilteredVideos = CollectionViewSource.GetDefaultView(this.VideoCollection.OrderBy(v => v.Title));
            this.InfoVisibility = Visibility.Visible;

            this.EditCommand = new DelegateCommand(this.Edit, this.CanEdit);
            this.AddVideoCommand = new DelegateCommand(this.Add);
            this.PlayAllCommand = new DelegateCommand(this.PlayAll);
            this.PlayOneCommand = new DelegateCommand(this.PlayOne);
        }

        public DelegateCommand AddVideoCommand { get; set; }

        public DelegateCommand PlayOneCommand { get; set; }

        public DelegateCommand PlayAllCommand { get; set; }

        public Visibility InfoVisibility
        {
            get { return this._infoVisibility; }
            set
            {
                if (value == this._infoVisibility) return;
                this._infoVisibility = value;
                this.OnPropertyChanged();
            }
        }

        public DelegateCommand EditCommand { get; set; }

        public ICollectionView FilteredVideos
        {
            get { return this._filteredVideos; }
            set
            {
                if (Equals(value, this._filteredVideos))
                {
                    return;
                }
                this._filteredVideos = value;
                if (this.CurrentVideo == null)
                {
                    this.CurrentVideo = this.VideoCollection.OrderBy(t => t.Title).First();
                }
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
                    this.OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Video> VideoCollection
        {
            get { return this._videoCollection; }
            set
            {
                if (!Equals(value, this._videoCollection))
                {
                    this._videoCollection = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private void PlayOne()
        {
            this._eventAggregator.GetEvent<PlayOneEvent>().Publish(this.CurrentVideo);
        }

        private void PlayAll()
        {
            this._eventAggregator.GetEvent<PlayAllEvent>().Publish(null);
        }

        private Boolean CanEdit()
        {
            return true;
        }

        private void Edit()
        {
            this.InfoVisibility = this.InfoVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Add()
        {
            this._eventAggregator.GetEvent<VideoAddedEvent>().Publish(this.CurrentVideo);
        }

        private void FilterCategory(String category)
        {
            ICollectionView view = this.FilteredVideos;

            view.Filter = item =>
            {
                Boolean result = true;
                try
                {
                    var video = item as Video;
                    Boolean isCategoryOk = this.IsCategoryOk(video, category);
                    result = isCategoryOk;
                }
                catch (Exception e)
                {
                    this.Logger().ErrorFormat(e.Message);
                }
                return result;
            };
        }

        private Boolean IsCategoryOk(Video video, String category)
        {
            Boolean isCategoryOk;
            if (String.Compare(category, "all", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                isCategoryOk = true;
            }
            else if (String.IsNullOrEmpty(category))
            {
                isCategoryOk = String.IsNullOrEmpty(video.Category);
            }
            else
            {
                isCategoryOk = !String.IsNullOrEmpty(video.Category) && video.Category == category;
            }
            return isCategoryOk;
        }
    }
}