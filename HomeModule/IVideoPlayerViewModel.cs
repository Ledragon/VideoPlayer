using System;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using VideoPlayer.Infrastructure;

namespace HomeModule
{
    public interface IVideoPlayerViewModel : IViewModel
    {
        Int32 SelectedTab { get; set; }
        Visibility IsLoading { get; set; }
        DelegateCommand GoToHomePageCommand { get; set; }
        DelegateCommand ToggleStyleCommand { get; set; }
        DelegateCommand WindowClosingCommand { get; set; }
        DelegateCommand WindowLoadedCommand { get; set; }
        DelegateCommand ToggleExitMenuCommand { get; set; }
        WindowStyle WindowStyle { get; set; }
        ObservableCollection<Object> ExitMenu { get; set; }
        Visibility IsExitMenuVisible { get; set; }
    }
}