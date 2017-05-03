using System;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideoPlayer.ViewModels
{
    public interface IVideoPlayerViewModel : IViewModel
    {
        String LoadingMessage { get; }
        Visibility IsLoading { get; set; }
        DelegateCommand GoToHomePageCommand { get; }
        DelegateCommand ToggleStyleCommand { get; }
        DelegateCommand CloseCommand { get; }
        Visibility IsExitMenuVisible { get; set; }
    }
}