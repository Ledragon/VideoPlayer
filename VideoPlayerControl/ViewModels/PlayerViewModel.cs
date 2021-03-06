﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Classes;
using VideoPlayerControl.Commands;

namespace VideoPlayerControl.ViewModels
{
    public class PlayerViewModel : ViewModelBase
    {
        public delegate void PlayedEventHandler(Object sender, EventArgs e);

        public delegate void PositionChangedEventHandler(Object sender, EventArgs e);

        public delegate void RateChangedEventHandler(Object sender, EventArgs e);

        public delegate void StoppedEventHandler(Object sender, EventArgs e);

        public delegate void VideoChangedEventHandler(Object sender, EventArgs e);

        private readonly Timer _timer = new Timer();

        private Visibility _controlsVisibility;

        private Video _currentVideo;
        private Cursor _cursor;
        private ICommand _decreaseRateCommand;
        private TimeSpan _duration;
        private ICommand _increaseRateCommand;
        private Int32 _index;
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


        public PlayerViewModel()
        {
            this.Rate = 1;
            this.Playlist = new ObservableCollection<Video>();
            this.IsRepeat = true;
            this.ControlsVisibility = Visibility.Visible;
            this._timer.Interval = 500;
            this._timer.Elapsed += this.TimerOnElapsed;
            this._timer.Start();
            this.InitCommands();
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
                if (Equals(value, this._currentVideo)) return;
                this._currentVideo = value;
                this.Duration = value.Length;
                this.Title = value.Title;
                this.OnVideoChanged(new EventArgs());
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
                Double ticks = this._duration.Ticks*this._position;
                Int64 parsedTicks = Int64.Parse(ticks.ToString("0"));
                this.PositionTimeSpan = new TimeSpan(parsedTicks);
                this.OnPositionChanged(new EventArgs());
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
                this.OnPropertyChanged();
                this.OnRateChanged(new EventArgs());
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

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (DateTime.Now - this._mouseLastMouveDateTime > new TimeSpan(0, 0, 2))
            {
                this.Cursor = Cursors.None;
                this.ControlsVisibility = Visibility.Hidden;
            }
        }

        private void InitCommands()
        {
            this.IncreaseRateCommand = new IncreaseRateCommand(this.IncreaseRate);
            this.DecreaseRateCommand = new DecreaseRateCommand(this.DecreaseRate);
            this.NextCommand = new NextCommand(this.Next);
            this.PreviousCommand = new PreviousCommand(this.Previous);
            this.StopCommand = new StopCommand(this.Stop);
            this.MouseMoveCommand = new MouseMoveCommand(this.MouseMove);
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
            this.OnStopped(new EventArgs());
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
            this.AddVideo(video);
            this.CurrentVideo = video;
            this.OnPlayed(new EventArgs());
        }

        public void PlayAll()
        {
            this.OnPlayed(new EventArgs());
        }

        public void Next()
        {
            this._index++;
            if (this._index == this.Playlist.Count - 1 && !this.IsRepeat)
            {
                this.NextCommand.CanExecute(false);
            }
            else if (this._index == this.Playlist.Count)
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

        #region Events

        //FOllowing events to be removed  when proper binding can be done
        public event PositionChangedEventHandler PositionChanged;

        private void OnPositionChanged(EventArgs e)
        {
            if (this.PositionChanged != null)
            {
                this.PositionChanged(this, e);
            }
        }

        public event VideoChangedEventHandler VideoChanged;

        private void OnVideoChanged(EventArgs e)
        {
            if (this.VideoChanged != null)
            {
                this.VideoChanged(this, e);
            }
        }

        public event RateChangedEventHandler RateChanged;

        private void OnRateChanged(EventArgs e)
        {
            if (this.RateChanged != null)
            {
                this.RateChanged(this, e);
            }
        }

        public event StoppedEventHandler Stopped;

        private void OnStopped(EventArgs e)
        {
            if (this.Stopped != null)
            {
                this.Stopped(this, e);
            }
        }

        public event PlayedEventHandler Played;

        private void OnPlayed(EventArgs e)
        {
            if (this.Played != null)
            {
                this.Played(this, e);
            }
        }

        #endregion
    }
}