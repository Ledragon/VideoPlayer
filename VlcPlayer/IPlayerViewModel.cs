using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Classes;
using VideoPlayer.Infrastructure;

namespace VlcPlayer
{
    public interface IPlayerViewModel : IViewModel
    {
        Visibility ControlsVisibility { get; set; }
        Cursor Cursor { get; set; }

        ICommand MouseMoveCommand { get; set; }
        ICommand StopCommand { get; set; }
        ICommand PreviousCommand { get; set; }
        ICommand IncreaseRateCommand { get; set; }
        ICommand DecreaseRateCommand { get; set; }
        ICommand NextCommand { get; set; }

        Boolean IsMouseDown { get; set; }
        Boolean IsRepeat { get; set; }
        Boolean IsMute { get; set; }
        Boolean IsPaused { get; set; }

        ObservableCollection<Video> Playlist { get; set; }
        TimeSpan TimePosition { get; set; }
        Video CurrentVideo { get; set; }
        String TemporaryImagePath { get; }
        String Title { get; set; }
        Single Position { get; set; }
        Single Rate { get; set; }
        TimeSpan Duration { get; set; }
        TimeSpan PositionTimeSpan { get; set; }
        ImageSource Source { get; set; }

        void MouseMove();
        void AddVideo(String path);
        void AddVideo(Video video);
        void ClearPlaylist();
        void PlayVideo(Video video);
        void PlayAll(IEnumerable<Video> playlist);
        void Next(Object dummy);
        void Next();
        void Previous();
    }
}