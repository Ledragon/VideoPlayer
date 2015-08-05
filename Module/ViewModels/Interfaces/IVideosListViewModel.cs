using System;
using System.ComponentModel;
using System.Windows;
using Classes;
using Microsoft.Practices.Prism.Commands;
using VideoPlayer.Infrastructure;

namespace Module.Interfaces
{
    public interface IVideosListViewModel : IViewModel
    {
        Video CurrentVideo { get; set; }
        //ObservableCollection<Video> VideoCollection { get; set; }
        DelegateCommand AddVideoCommand { get; }
        DelegateCommand EditCommand { get; }
        DelegateCommand PlayOneCommand { get; }
        DelegateCommand PlayPlaylistCommand { get; }
        Visibility InfoVisibility { get; set; }
        ICollectionView FilteredVideos { get; set; }
        Int32 EditIndex { get; set; }
    }
}