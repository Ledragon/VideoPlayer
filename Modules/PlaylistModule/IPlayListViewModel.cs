﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Classes;
using VideoPlayer.Infrastructure;

namespace PlaylistModule
{
    public interface IPlayListViewModel : IViewModel
    {
        ObservableCollection<Video> Playlist { get; set; }
        Video CurrentVideo { get; set; }
        TimeSpan TotalDuration { get; set; }
        ICommand RemoveCommand { get; }
        ICommand AddCommand { get; }
        ICommand AddRangeCommand { get; }
    }
}