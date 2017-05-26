using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;
using Classes;
using Log;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;
using VideoPlayer.Infrastructure.ViewFirst;
using VideoPlayer.Services;

namespace VlcPlayer
{
    public class PlayerViewModel : ViewModelBase, IPlayerViewModel, IActiveAware
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IPlaylistService _playlistService;
        private readonly Timer _timer = new Timer();
        private Boolean _autoPlay;
        private Boolean _controlsVisibility;
        private Video _currentVideo;
        private Cursor _cursor;
        private TimeSpan _duration;
        private Int32 _index;
        private Boolean _isActive;
        private Boolean _isMouseDown;
        private Boolean _isMute;
        private Boolean _isPaused;
        private Boolean _isRepeat;
        private DateTime _mouseLastMouveDateTime = DateTime.Now;
        private ObservableCollection<Video> _playlist;
        private Boolean _playlistVisibility;
        private Single _position;
        private TimeSpan _positionTimeSpan;
        private Single _rate;
        private ImageSource _source;
        private TimeSpan _timePosition;
        private String _title;

        public PlayerViewModel(IEventAggregator eventAggregator, IPlaylistService playlistService)
        {
            this._eventAggregator = eventAggregator;
            this._playlistService = playlistService;
            this.Rate = 1;
            this.IsRepeat = false;
            this.ControlsVisibility = true;
            this._timer.Interval = 500;
            this._timer.Elapsed += this.TimerOnElapsed;
            this._timer.Start();
            this.InitCommands();
            this._autoPlay = true;
            eventAggregator.GetEvent<VideoDurationChanged>()
                .Subscribe(this.VideoDurationChanged);
            eventAggregator.GetEvent<VideoEnded>()
                .Subscribe(this.Next);
            eventAggregator.GetEvent<SetVideo>()
                .Subscribe(v =>
                {
                    this._autoPlay = false;
                    this.CurrentVideo = v;
                });
        }

        public Boolean PlaylistVisibility
        {
            get { return this._playlistVisibility; }
            set
            {
                if (value == this._playlistVisibility)
                {
                    return;
                }
                this._playlistVisibility = value;
                this.OnPropertyChanged();
            }
        }

        public static String AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public Boolean IsActive
        {
            get { return this._isActive; }
            set
            {
                this._isActive = value;
                if (value)
                {
                    if (this._playlistService.Playlist != null)
                    {
                        this.Playlist = new ObservableCollection<Video>(this._playlistService.Playlist);
                        if (this.Playlist.Any())
                        {
                            this.CurrentVideo = this.Playlist.First();
                        }
                    }
                }
            }
        }

        public event EventHandler IsActiveChanged;

        public Boolean IsMouseDown
        {
            get { return this._isMouseDown; }
            set
            {
                if (value.Equals(this._isMouseDown))
                {
                    return;
                }
                this._isMouseDown = value;
                this.OnPropertyChanged();
            }
        }

        public Boolean ControlsVisibility
        {
            get { return this._controlsVisibility; }
            set
            {
                if (value == this._controlsVisibility)
                {
                    return;
                }
                this._controlsVisibility = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand MouseMoveCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }
        public ICommand IncreaseRateCommand { get; private set; }
        public ICommand DecreaseRateCommand { get; private set; }
        public ICommand NextCommand { get; private set; }

        public Boolean IsRepeat
        {
            get { return this._isRepeat; }
            set
            {
                if (value.Equals(this._isRepeat))
                {
                    return;
                }
                this._isRepeat = value;
                this.OnPropertyChanged();
            }
        }

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

        public TimeSpan TimePosition
        {
            get { return this._timePosition; }
            set
            {
                if (value.Equals(this._timePosition))
                {
                    return;
                }
                this._timePosition = value;
                this.OnPropertyChanged();
            }
        }

        public Video CurrentVideo
        {
            get { return this._currentVideo; }
            set
            {
                if (value != null)
                {
                    this.Duration = value.Length;
                    this.Title = value.Title;
                }
                //if (Equals(value, this._currentVideo)) return;
                this._currentVideo = value;
                if (this._autoPlay)
                {
                    this._eventAggregator.GetEvent<PlayedEvent>().Publish(this.CurrentVideo);
                }
                this.OnPropertyChanged();
            }
        }

        public String TemporaryImagePath => Path.Combine(AssemblyDirectory, this.Title + ".jpg");

        public String Title
        {
            get { return this._title; }
            set
            {
                if (value == this._title)
                {
                    return;
                }
                this._title = value;
                this.OnPropertyChanged();
            }
        }

        public Single Position
        {
            get { return this._position; }
            set
            {
                if (value.Equals(this._position))
                {
                    return;
                }
                this._position = value;
                if (this._position > 1)
                {
                    this._position = 1;
                }
                else if (this._position < 0)
                {
                    this._position = 0;
                }
                Double ticks = this._duration.Ticks*this._position;
                var parsedTicks = Int64.Parse(ticks.ToString("0"));
                this.PositionTimeSpan = new TimeSpan(parsedTicks);
                if (this.IsMouseDown)
                {
                    this._eventAggregator.GetEvent<VideoPositionChanged>().Publish(this._position);
                }
                this.OnPropertyChanged();
            }
        }

        public Single Rate
        {
            get { return this._rate; }
            set
            {
                if (value.Equals(this._rate))
                {
                    return;
                }
                this._rate = value;
                this._eventAggregator.GetEvent<RateChanged>().Publish(this._rate);
                this.OnPropertyChanged();
            }
        }

        public TimeSpan Duration
        {
            get { return this._duration; }
            set
            {
                if (value.Equals(this._duration))
                {
                    return;
                }
                this._duration = value;
                this.OnPropertyChanged();
            }
        }

        public TimeSpan PositionTimeSpan
        {
            get { return this._positionTimeSpan; }
            set
            {
                if (value.Equals(this._positionTimeSpan))
                {
                    return;
                }
                this._positionTimeSpan = value;
                this.OnPropertyChanged();
            }
        }

        public Boolean IsMute
        {
            get { return this._isMute; }
            set
            {
                if (value.Equals(this._isMute))
                {
                    return;
                }
                this._isMute = value;
                this.OnPropertyChanged();
            }
        }

        public ImageSource Source
        {
            get { return this._source; }
            set
            {
                if (Equals(value, this._source))
                {
                    return;
                }
                this._source = value;
                this.OnPropertyChanged();
            }
        }

        public Boolean IsPaused
        {
            get { return this._isPaused; }
            set
            {
                if (value.Equals(this._isPaused))
                {
                    return;
                }
                this._isPaused = value;
                this.OnPropertyChanged();
            }
        }

        public Cursor Cursor
        {
            get { return this._cursor; }
            set
            {
                if (Equals(value, this._cursor))
                {
                    return;
                }
                this._cursor = value;
                this.OnPropertyChanged();
            }
        }

        public void MouseMove()
        {
            this._mouseLastMouveDateTime = DateTime.Now;
            this.Cursor = Cursors.Arrow;
            this.ControlsVisibility = true;
            this.PlaylistVisibility = this.Playlist != null && this.Playlist.Count > 1;
        }

        public void Next(Object dummy)
        {
            this.Next();
        }

        public void Next()
        {
            this._index++;
            if (this._index >= this.Playlist.Count && !this.IsRepeat)
            {
                this.NextCommand.CanExecute(false);
                this._eventAggregator.GetEvent<PlayCompleted>().Publish(null);
            }
            else
            {
                if (this._index == this.Playlist.Count)
                {
                    this._index = 0;
                }
                this.CurrentVideo = this.Playlist[this._index];
            }
        }

        public void Previous()
        {
            this._index--;
            if (this._index == 0 && !this.IsRepeat)
            {
                this.PreviousCommand.CanExecute(false);
            }
            else if (this._index < 0)
            {
                this._index = this.Playlist.Count - 1;
            }
            this.CurrentVideo = this.Playlist[this._index];
        }

        public void AddVideo(String path)
        {
            var video = new Video(path);
            this.AddVideo(video);
        }

        public void AddVideo(Video video)
        {
            this.Logger().DebugFormat("Adding video '{0}' to playlist.", video.Title);
            this.Playlist.Add(video);
            if (this._currentVideo == null)
            {
                this._currentVideo = video;
            }
        }

        private void VideoDurationChanged(TimeSpan span)
        {
            if (this.CurrentVideo.Length == TimeSpan.Zero)
            {
                this.CurrentVideo.Length = span;
                this.Duration = span;
            }
        }

        private void TimerOnElapsed(Object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (DateTime.Now - this._mouseLastMouveDateTime > new TimeSpan(0, 0, 2))
            {
                this.Cursor = Cursors.None;
                this.ControlsVisibility = false;
            }
        }

        private void InitCommands()
        {
            this.IncreaseRateCommand = new DelegateCommand(this.IncreaseRate);
            this.DecreaseRateCommand = new DelegateCommand(this.DecreaseRate);
            this.NextCommand = new DelegateCommand(this.Next);
            this.PreviousCommand = new DelegateCommand(this.Previous);
            this.StopCommand = new DelegateCommand(this.Stop);
            this.MouseMoveCommand = new DelegateCommand(this.MouseMove);
        }

        private void Stop()
        {
            this.IsPaused = false;
            this.Playlist?.Clear();
            this._eventAggregator.GetEvent<OnStop>().Publish(null);
        }

        private void DecreaseRate()
        {
            this.Rate /= 2;
        }

        private void IncreaseRate()
        {
            this.Rate *= 2;
        }
    }
}