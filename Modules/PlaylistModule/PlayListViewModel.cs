using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Classes;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;

namespace PlaylistModule
{
    public class PlayListViewModel : ViewModelBase, IPlayListViewModel
    {
        private readonly ILibraryService _libraryService;
        private Video _currentVideo;
        private ObservableCollection<Video> _playlist;
        private ObservableCollection<Playlist> _playListCollection;
        private String _playListName;
        private Playlist _selectedPlayList;
        private TimeSpan _totalDuration;

        public PlayListViewModel(IPlayListView view, IEventAggregator eventAggregator, ILibraryService libraryService)
            : base(view)
        {
            this._libraryService = libraryService;
            this.Playlist = new ObservableCollection<Video>();

            this.RemoveCommand = new DelegateCommand(() => this.Remove(this.CurrentVideo));

            this.AddCommand = new DelegateCommand<Video>(this.Add);
            this.AddRangeCommand = new DelegateCommand<IEnumerable<Video>>(this.Add);
            this.SavePlaylistCommand = new DelegateCommand(this.Save);

            eventAggregator.GetEvent<OnAddVideo>().Subscribe(this.Add);
            eventAggregator.GetEvent<OnAddVideoRange>().Subscribe(this.Add);

            eventAggregator.GetEvent<OnPlayPlaylistRequest>()
                .Subscribe(dummy => { eventAggregator.GetEvent<OnPlayPlaylist>().Publish(this.Playlist); }, true);

            this._playListCollection = new ObservableCollection<Playlist>(libraryService.GetObjectsFromFile().PlayLists);
        }

        public String PlayListName
        {
            get { return this._playListName; }
            set
            {
                if (value == this._playListName)
                {
                    return;
                }
                this._playListName = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<Playlist> PlayListCollection
        {
            get { return this._playListCollection; }
            set
            {
                if (Equals(value, this._playListCollection))
                {
                    return;
                }
                this._playListCollection = value;
                this.OnPropertyChanged();
            }
        }

        public Playlist SelectedPlayList
        {
            get { return this._selectedPlayList; }
            set
            {
                if (Equals(value, this._selectedPlayList))
                {
                    return;
                }
                this._selectedPlayList = value;
                this.OnPropertyChanged();
                this.Playlist.Clear();
                this.Playlist.AddRange(this._libraryService.GetVideosByFilePath(this._selectedPlayList.Files));
                this.TotalDuration = this.Playlist.Aggregate(TimeSpan.Zero, (current, video) => current.Add(video.Length));
            }
        }

        public ICommand SavePlaylistCommand { get; private set; }

        public ObservableCollection<Video> Playlist
        {
            get { return this._playlist; }
            set
            {
                if (Equals(value, this._playlist))
                {
                    return;
                }
                this._playlist = value;
                this.OnPropertyChanged();
            }
        }

        public Video CurrentVideo
        {
            get { return this._currentVideo; }
            set
            {
                if (Equals(value, this._currentVideo))
                {
                    return;
                }
                this._currentVideo = value;
                this.OnPropertyChanged();
            }
        }

        public TimeSpan TotalDuration
        {
            get
            {
                return this._totalDuration;
            }
            set
            {
                if (value.Equals(this._totalDuration))
                {
                    return;
                }
                this._totalDuration = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand RemoveCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand AddRangeCommand { get; }

        private void Add(IEnumerable<Video> videos)
        {
            this.Playlist.AddRange(videos.Where(v => !this.Playlist.Contains(v)));
            this.TotalDuration = this.Playlist.Aggregate(TimeSpan.Zero, (current, v) => current.Add(v.Length));
        }

        private void Add(Video video)
        {
            if (!this.Playlist.Contains(video))
            {
                this.Playlist.Add(video);
                this.TotalDuration = this.TotalDuration.Add(video.Length);
            }
        }

        private void Remove(Video video)
        {
            if (this.Playlist.Contains(video))
            {
                this.Playlist.Remove(video);
                this.TotalDuration = this.TotalDuration.Subtract(video.Length);
            }
        }

        private void Save()
        {
            var playlist = new Playlist
            {
                Title = this.PlayListName,
                Files = this.Playlist.Select(v => v.FileName).ToList()
            };
            this._libraryService.AddPlaylist(playlist);
        }
    }
}