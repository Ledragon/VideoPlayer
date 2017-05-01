using System;
using System.Windows.Input;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideoPlayer.PlaylistManagement
{
    public interface IPlayListManagementViewModel : IViewModel
    {
        Boolean IsCategoryGridVisible { get; set; }
        Boolean IsPlayListVisible { get; set; }
        Boolean FilterGridVisibility { get; set; }
        ICommand SwitchCategoryGridVisibilityCommand { get; }
        ICommand SwitchPlaylistVisibilityCommand { get; }
        ICommand SwitchFilterGridVisibilityCommand { get; }
    }
}