using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Classes;
using Log;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Infrastructure;

namespace VlcPlayer
{
    public class PlayerViewModel : ViewModelBase, IPlayerViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly Timer _timer = new Timer();
        private Visibility _controlsVisibility;
        private Video _currentVideo;
        private Cursor _cursor;
        private ICommand _decreaseRateCommand;
        private TimeSpan _duration;
        private ICommand _increaseRateCommand;
        private Int32 _index;
        private Boolean _isMouseDown;
        private Boolean _isMute;
        private Boolean _isPaused;
        private Boolean _isRepeat;
        private DateTime _mouseLastMouveDateTime = DateTime.Now;
        private ICommand _mouseMoveCommand;
        private ICommand _nextCommand;
        private Single _position;
        private TimeSpan _positionTimeSpan;
        private ICommand _previousCommand;
        private Single _rate;
        private ImageSource _source;
        private ICommand _stopCommand;
        private TimeSpan _timePosition;
        private String _title;

        public PlayerViewModel(IPlayer player, IEventAggregator eventAggregator) : base(player)
        {
            this._eventAggregator = eventAggregator;
            this.Rate = 1;
            this.Playlist = new ObservableCollection<Video>();
            this.IsRepeat = true;
            this.ControlsVisibility = Visibility.Visible;
            this._timer.Interval = 500;
            this._timer.Elapsed += this.TimerOnElapsed;
            this._timer.Start();
            this.InitCommands();

            eventAggregator.GetEvent<VideoAddedEvent>().Subscribe(this.AddVideo);
            eventAggregator.GetEvent<PlayPlaylistEvent>().Subscribe(this.PlayAll);
            eventAggregator.GetEvent<PlayOneEvent>().Subscribe(this.PlayVideo);
            eventAggregator.GetEvent<VideoDurationChanged>().Subscribe(this.VideoDurationChanged);
            eventAggregator.GetEvent<VideoEnded>().Subscribe(this.Next);
            eventAggregator.GetEvent<ClearPlaylistEvent>().Subscribe(this.ClearPlaylist);
        }

        public Boolean IsMouseDown
        {
            get { return this._isMouseDown; }
            set
            {
                if (value.Equals(this._isMouseDown)) return;
                this._isMouseDown = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand MouseMoveCommand
        {
            get { return this._mouseMoveCommand; }
            set
            {
                if (Equals(value, this._mouseMoveCommand)) return;
                this._mouseMoveCommand = value;
                this.OnPropertyChanged();
            }
        }

        public Visibility ControlsVisibility
        {
            get { return this._controlsVisibility; }
            set
            {
                if (value == this._controlsVisibility) return;
                this._controlsVisibility = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand StopCommand
        {
            get { return this._stopCommand; }
            set
            {
                if (Equals(value, this._stopCommand)) return;
                this._stopCommand = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand PreviousCommand
        {
            get { return this._previousCommand; }
            set
            {
                if (Equals(value, this._previousCommand)) return;
                this._previousCommand = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand IncreaseRateCommand
        {
            get { return this._increaseRateCommand; }
            set
            {
                if (Equals(value, this._increaseRateCommand)) return;
                this._increaseRateCommand = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand DecreaseRateCommand
        {
            get { return this._decreaseRateCommand; }
            set
            {
                if (Equals(value, this._decreaseRateCommand)) return;
                this._decreaseRateCommand = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand NextCommand
        {
            get { return this._nextCommand; }
            set
            {
                if (Equals(value, this._nextCommand)) return;
                this._nextCommand = value;
                this.OnPropertyChanged();
            }
        }

        public Boolean IsRepeat
        {
            get { return this._isRepeat; }
            set
            {
                if (value.Equals(this._isRepeat)) return;
                this._isRepeat = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<Video> Playlist { get; set; }

        public TimeSpan TimePosition
        {
            get { return this._timePosition; }
            set
            {
                if (value.Equals(this._timePosition)) return;
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
                if (Equals(value, this._currentVideo)) return;
                this._currentVideo = value;
                this._eventAggregator.GetEvent<PlayedEvent>().Publish(this.CurrentVideo);
                this.OnPropertyChanged();
            }
        }

        public String TemporaryImagePath
        {
            get { return Path.Combine(Environment.GetEnvironmentVariable("TEMP"), this.Title + ".jpg"); }
        }

        public String Title
        {
            get { return this._title; }
            set
            {
                if (value == this._title) return;
                this._title = value;
                this.OnPropertyChanged();
            }
        }

        public Single Position
        {
            get { return this._position; }
            set
            {
                if (value.Equals(this._position)) return;
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
                if (value.Equals(this._rate)) return;
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
                if (value.Equals(this._duration)) return;
                this._duration = value;
                this.OnPropertyChanged();
            }
        }

        public TimeSpan PositionTimeSpan
        {
            get { return this._positionTimeSpan; }
            set
            {
                if (value.Equals(this._positionTimeSpan)) return;
                this._positionTimeSpan = value;
                this.OnPropertyChanged();
            }
        }

        public Boolean IsMute
        {
            get { return this._isMute; }
            set
            {
                if (value.Equals(this._isMute)) return;
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
                if (value.Equals(this._isPaused)) return;
                this._isPaused = value;
                this.OnPropertyChanged();
            }
        }

        public Cursor Cursor
        {
            get { return this._cursor; }
            set
            {
                if (Equals(value, this._cursor)) return;
                this._cursor = value;
                this.OnPropertyChanged();
            }
        }

        private void ClearPlaylist(Object obj)
        {
            this.ClearPlaylist();
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
                this.ControlsVisibility = Visibility.Hidden;
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

        public void MouseMove()
        {
            this._mouseLastMouveDateTime = DateTime.Now;
            this.Cursor = Cursors.Arrow;
            this.ControlsVisibility = Visibility.Visible;
        }

        private void Stop()
        {
            this.IsPaused = false;
            this.Playlist.Clear();
            this._eventAggregator.GetEvent<StoppedEvent>().Publish(null);
        }

        private void DecreaseRate()
        {
            this.Rate /= 2;
        }

        private void IncreaseRate()
        {
            this.Rate *= 2;
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

        public void ClearPlaylist()
        {
            this.Playlist.Clear();
        }

        public void PlayVideo(Video video)
        {
            this.ClearPlaylist();
            if (video != null)
            {
                this.AddVideo(video);
                this.CurrentVideo = video;
                this._eventAggregator.GetEvent<PlayedEvent>().Publish(video);
            }
            else
            {
                this.Logger().WarnFormat("No video to play.");
            }
        }

        public void PlayAll(Object dummy)
        {
            if (this.Playlist.Any())
            {
                this.Logger().DebugFormat("Playing created playlist.");
                this.CurrentVideo = this.Playlist.First();
                this._eventAggregator.GetEvent<PlayedEvent>().Publish(this.CurrentVideo);
            }
            else
            {
                this.Logger().WarnFormat("No video in playlist.");
            }
        }

        public void Next(Object dummy)
        {
            this.Next();
        }

        public void Next()
        {
            this._index++;
            if (this._index == this.Playlist.Count - 1 && !this.IsRepeat)
            {
                this.NextCommand.CanExecute(false);
            }
            else if (this._index >= this.Playlist.Count)
            {
                this._index = 0;
            }
            this.CurrentVideo = this.Playlist[this._index];
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
    }
}