using System;
using System.Windows.Input;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.ViewModels
{
    public interface IVideosPageViewModel : IViewModel
    {
        Cursor Cursor { get; set; }
        //Boolean FilterGridVisibility { get; set; }
        //Boolean IsCategoryGridVisible { get; set; }
        //Boolean IsPlayListVisible { get; set; }
        Int32 SelectedIndex { get; set; }
        //ICommand SwitchCategoryGridVisibilityCommand { get; }
        //ICommand SwitchFilterGridVisibilityCommand { get; }
        //ICommand SwitchPlaylistVisibilityCommand { get; }
        ICommand SwitchToFullScreenCommand { get; }
        ICommand SwitchToWindowCommand { get; }
    }
}