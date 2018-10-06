using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Classes;
using VideoPlayer.Infrastructure.ViewFirst;

namespace PlaylistModule
{
    public interface IPlayListViewModel : IViewModel
    {
        ObservableCollection<Video> Playlist { get; set; }
        Video CurrentVideo { get; set; }
        TimeSpan TotalDuration { get; }
        ICommand RemoveCommand { get; }
        ICommand AddCommand { get; }
        ICommand AddRangeCommand { get; }
    }
}