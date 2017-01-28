using System;
//using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Classes;
using Microsoft.Practices.Prism.Commands;
using VideoPlayer.Infrastructure;
using VideosListModule.ViewModels;

namespace VideosListModule
{
    public interface IVideosListViewModel : IViewModel
    {
        Video CurrentVideo { get; set; }
        //ObservableCollection<Video> VideoCollection { get; set; }
        DelegateCommand AddVideoCommand { get; }
        //DelegateCommand EditCommand { get; }
        DelegateCommand PlayOneCommand { get; }
        DelegateCommand PlayPlaylistCommand { get; }
        Visibility InfoVisibility { get; set; }
        VideosCollectionView FilteredVideos { get; set; }
        Int32 EditIndex { get; set; }
        Boolean IsLoading { get; set; }
        DelegateCommand NextCommand { get; }
        DelegateCommand PreviousCommand { get; }
        Task Init();
    }
}