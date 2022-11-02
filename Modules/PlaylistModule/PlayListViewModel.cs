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
using VideoPlayer.Infrastructure.ViewFirst;
using VideoPlayer.Services;

namespace PlaylistModule
{
    public class PlayListViewModel : ViewModelBase, IPlayListViewModel
    {
        private readonly ILibraryService _libraryService;
        private readonly IPlaylistService _playlistService;
        private VideoViewModel _currentVideo;
        private ObservableCollection<VideoViewModel> _playlist;
        private ObservableCollection<Playlist> _playListCollection;
        private String _playListName;
        private Playlist _selectedPlayList;
        private TimeSpan _totalDuration;

        public PlayListViewModel(IEventAggregator eventAggregator, ILibraryService libraryService,
            IPlaylistService playlistService)
        {
            this._libraryService = libraryService;
            this._playlistService = playlistService;
            this.Playlist = new ObservableCollection<VideoViewModel>();

            this.RemoveCommand = new DelegateCommand(() => this.Remove(this.CurrentVideo));

            this.AddCommand = new DelegateCommand<VideoViewModel>(this.Add);
            this.AddRangeCommand = new DelegateCommand<IEnumerable<VideoViewModel>>(this.Add);
            this.SavePlaylistCommand = new DelegateCommand(this.Save);
            this.ClearCommand = new DelegateCommand(this.ClearPlaylist);

            eventAggregator.GetEvent<OnAddVideo>().Subscribe(this.Add);
            eventAggregator.GetEvent<OnAddVideoRange>().Subscribe(this.Add);

            eventAggregator.GetEvent<PlayRangeEvent>()
                .Subscribe(videos =>
                {
                    this.ClearPlaylist();
                    this.Add(videos);
                    playlistService.Playlist = this.Playlist;
                    eventAggregator.GetEvent<OnPlayPlaylistRequest>().Publish(this.Playlist);
                });

            eventAggregator.GetEvent<OnPlayPlaylistRequest>()
                .Subscribe(dummy => { eventAggregator.GetEvent<OnPlayPlaylist>().Publish(this.Playlist); }, true);

            this.PlayListCollection = new ObservableCollection<Playlist>(libraryService.GetObjectsFromFile().PlayLists);
            playlistService.Playlist = this.Playlist;
            this.Playlist.CollectionChanged += (s, e) =>
            {
                this.TotalDuration = this.Playlist
                    .Aggregate(TimeSpan.Zero, (current, v) => current.Add(v.Length));
            };
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
                this.Playlist.AddRange(
                    this._libraryService.GetVideosByFilePath(this._selectedPlayList.Items.Select(f => f.FileName)));
                this.PlayListName = this._selectedPlayList.Title;
                this.TotalDuration = this.Playlist.Aggregate(TimeSpan.Zero,
                    (current, video) => current.Add(video.Length));
            }
        }


        public ObservableCollection<VideoViewModel> Playlist
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

        public VideoViewModel CurrentVideo
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
            get { return this._totalDuration; }
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


        public ICommand ClearCommand { get; }
        public ICommand SavePlaylistCommand { get; private set; }
        public ICommand RemoveCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand AddRangeCommand { get; }

        private void Add(IEnumerable<VideoViewModel> videos)
        {
            this.Playlist.AddRange(videos.Where(v => !this.Playlist.Contains(v)));
        }

        private void Add(VideoViewModel video)
        {
            if (!this.Playlist.Contains(video))
            {
                this.Playlist.Add(video);
            }
        }

        private void Remove(VideoViewModel video)
        {
            if (this.Playlist.Contains(video))
            {
                this.Playlist.Remove(video);
            }
        }

        private void Save()
        {
            var playlist = this.PlayListCollection.SingleOrDefault(p => p.Title == this._playListName);
            if (playlist == null)
            {
                playlist = new Playlist
                {
                    Title = this.PlayListName
                };
                this._libraryService.AddPlaylist(playlist);
                this.PlayListCollection.Add(playlist);
            }
            playlist.Items = this.Playlist.Select((v, i) => new PlayListItem(v.FileName, i)).ToList();
        }

        private void ClearPlaylist()
        {
            this.Playlist.Clear();
            this._playlistService.Clear();

        }
    }
}