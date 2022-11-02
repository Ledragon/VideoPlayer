using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using Classes;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VlcPlayer
{
    public interface IPlayerViewModel : IViewModel
    {
        Boolean ControlsVisibility { get; set; }
        Cursor Cursor { get; set; }
        ICommand MouseMoveCommand { get; }
        ICommand StopCommand { get; }
        ICommand PreviousCommand { get; }
        ICommand IncreaseRateCommand { get;  }
        ICommand DecreaseRateCommand { get;  }
        ICommand NextCommand { get;  }
        Boolean IsMouseDown { get; set; }
        Boolean IsRepeat { get; set; }
        Boolean IsMute { get; set; }
        Boolean IsPaused { get; set; }
        ObservableCollection<VideoViewModel> Playlist { get; set; }
        TimeSpan TimePosition { get; set; }
        VideoViewModel CurrentVideo { get; set; }
        String TemporaryImagePath { get; }
        String Title { get; set; }
        Single Position { get; set; }
        Single Rate { get; set; }
        TimeSpan Duration { get; set; }
        TimeSpan PositionTimeSpan { get; set; }
        ImageSource Source { get; set; }
        void MouseMove();
        //void AddVideo(String path);
        //void AddVideo(Video video);
        //void ClearPlaylist();
        //void PlayVideo(Video video);
        //void PlayAll(IEnumerable<Video> playlist);
        void Next(Object dummy);
        void Next();
        void Previous();
    }
}