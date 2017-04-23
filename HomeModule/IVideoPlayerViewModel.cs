using System;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using VideoPlayer.Infrastructure.ViewFirst;

namespace HomeModule
{
    public interface IVideoPlayerViewModel : IViewModel
    {
        Int32 SelectedTab { get; set; }
        String LoadingMessage { get; }
        Visibility IsLoading { get; set; }
        DelegateCommand GoToHomePageCommand { get; }
        DelegateCommand ToggleStyleCommand { get; }
        //DelegateCommand WindowLoadedCommand { get; }
        DelegateCommand CloseCommand { get; }
        //WindowStyle WindowStyle { get; set; }
        Visibility IsExitMenuVisible { get; set; }
    }
}