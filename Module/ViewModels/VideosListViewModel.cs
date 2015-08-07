using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Classes;
using Log;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Module.Interfaces;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;

namespace Module
{
    public class VideosListViewModel : ViewModelBase, IVideosListViewModel
    {
        private String _categoryFilter;
        private Video _currentVideo;
        private Int32 _editIndex;
        private IEventAggregator _eventAggregator;
        private ICollectionView _filteredVideos;
        private String _filterName;
        private List<String> _filterTags;
        private Visibility _infoVisibility;
        private SortDescription _sortDescription;

        public VideosListViewModel(ILibraryService libraryService, IVideosList videosList,
            IEventAggregator eventAggregator)
            : base(videosList)
        {
            this.Init(libraryService, eventAggregator);
        }

        public DelegateCommand AddVideoCommand { get; private set; }
        public DelegateCommand EditCommand { get; private set; }
        public DelegateCommand PlayOneCommand { get; private set; }
        public DelegateCommand PlayPlaylistCommand { get; private set; }

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

                this._eventAggregator.GetEvent<FilterChangedEvent>().Publish(this.FilteredVideos.Cast<Video>().Count());
                this.OnPropertyChanged();
            }
        }

        public Int32 EditIndex
        {
            get { return this._editIndex; }
            set
            {
                if (value == this._editIndex) return;
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
                    if (this.EditIndex == 1)
                    {
                        this._eventAggregator.GetEvent<VideoEditing>().Publish(this.CurrentVideo);
                    }
                    this.OnPropertyChanged();
                }
            }
        }

        private void Init(ILibraryService libraryService, IEventAggregator eventAggregator)
        {
            try
            {
                this._eventAggregator = eventAggregator;
                this._categoryFilter = "All";

                this._eventAggregator.GetEvent<SelectedCategoryChangedEvent>().Subscribe(this.FilterCategory);
                this._eventAggregator.GetEvent<NameFilterChangedEvent>().Subscribe(this.FilterName);
                this._eventAggregator.GetEvent<TagFilterChangedEvent>().Subscribe(this.FilterTag);
                this._eventAggregator.GetEvent<PlayAllEvent>().Subscribe(this.PlayAll);
                this._eventAggregator.GetEvent<SortingChangedEvent>().Subscribe(this.Sort);
                this._eventAggregator.GetEvent<LibraryUpdated>().Subscribe(this.UpdateVideoListView);
                this._eventAggregator.GetEvent<VideoEdited>().Subscribe(dummy => this.SetFilter());

                this.UpdateVideoListView(libraryService.GetObjectsFromFile().Videos);
                this.InfoVisibility = Visibility.Visible;

                this.EditCommand = new DelegateCommand(this.Edit, this.CanEdit);
                this.AddVideoCommand = new DelegateCommand(this.Add, this.CanCommandsExecute);
                this.PlayPlaylistCommand = new DelegateCommand(this.PlayPlaylist, this.CanCommandsExecute);
                this.PlayOneCommand = new DelegateCommand(this.PlayOne, this.CanCommandsExecute);
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
                throw;
            }
        }

        private void UpdateVideoListView(IEnumerable<Video> videos)
        {
            this._sortDescription = new SortDescription("Title", ListSortDirection.Ascending);
            var view = CollectionViewSource.GetDefaultView(videos);
            view.SortDescriptions.Add(this._sortDescription);
            this.FilteredVideos = view;
            if (this.CurrentVideo == null)
            {
                this.CurrentVideo = videos.OrderBy(v => v.Title).First();
            }
        }

        private void FilterTag(List<String> tags)
        {
            this._filterTags = tags;
            this.SetFilter();
        }

        private Boolean CanCommandsExecute()
        {
            return this.EditIndex == 0;
        }

        private void Sort(SortDescription obj)
        {
            if (this._sortDescription != obj)
            {
                this._sortDescription = obj;
                //this.FilteredVideos.DeferRefresh();
                this.FilteredVideos.SortDescriptions.Clear();
                this.FilteredVideos.SortDescriptions.Add(obj);
                this.FilteredVideos.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
                //this.FilteredVideos.Refresh();
            }
        }

        private void FilterName(String obj)
        {
            if (this._filterName != obj)
            {
                this._filterName = obj;
                this.SetFilter();
            }
        }

        private void FilterCategory(String category)
        {
            if (this._categoryFilter != category)
            {
                this._categoryFilter = category;
                this.SetFilter();
            }
        }

        private void SetFilter()
        {
            this.FilteredVideos.Filter = this.Filter();
            this._eventAggregator.GetEvent<FilterChangedEvent>().Publish(this.FilteredVideos.Cast<Video>().Count());
        }

        private Predicate<Object> Filter()
        {
            return item =>
            {
                var result = true;
                try
                {
                    var video = item as Video;
                    var isNameOk = this.IsNameOk(video);
                    var isCategoryOk = this.IsCategoryOk(video, this._categoryFilter);
                    var isTagOk = this.IsTagOK(video);
                    result = isNameOk && isCategoryOk && isTagOk;
                }
                catch (Exception e)
                {
                    this.Logger().ErrorFormat(e.Message);
                }
                return result;
            };
        }

        private void PlayAll(Object obj)
        {
            this._eventAggregator.GetEvent<ClearPlaylistEvent>().Publish(null);
            var videoAddedEvent = this._eventAggregator.GetEvent<VideoAddedEvent>();
            foreach (var video in this.FilteredVideos.Cast<Video>())
            {
                videoAddedEvent.Publish(video);
            }
            this.PlayPlaylist();
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
            this._eventAggregator.GetEvent<PlayPlaylistEvent>().Publish(null);
        }

        private Boolean CanEdit()
        {
            return true;
        }

        private void Edit()
        {
            if (this.EditIndex == 0)
            {
                this._eventAggregator.GetEvent<VideoEditing>().Publish(this.CurrentVideo);
                this.EditIndex = 1;
            }
            else
            {
                this._eventAggregator.GetEvent<VideoEdited>().Publish(null);
                this.EditIndex = 0;
            }
        }

        private void Add()
        {
            if (File.Exists(this.CurrentVideo.FileName))
            {
                this._eventAggregator.GetEvent<VideoAddedEvent>().Publish(this.CurrentVideo);
            }
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

        private Boolean IsNameOk(Video video)
        {
            var result = true;
            if (!String.IsNullOrEmpty(this._filterName))
            {
                result = video.Title.ToLower().Contains(this._filterName.ToLower());
            }
            return result;
        }

        private Boolean IsTagOK(Video video)
        {
            var result = true;
            if (this._filterTags != null && this._filterTags.Any())
            {
                result =
                    this._filterTags.Any(
                        filterTag =>
                            video.Tags.Any(
                                t => String.Equals(t.Value, filterTag, StringComparison.CurrentCultureIgnoreCase)));
            }
            return result;
        }
    }
}