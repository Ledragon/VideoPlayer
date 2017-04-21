using System;
using System.Windows.Input;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideosPageModule
{
    public interface IVideosPageViewModel : IViewModel
    {
        Cursor Cursor { get; set; }
        Int32 SelectedIndex { get; set; }
        ICommand SwitchToFullScreenCommand { get; }
        ICommand SwitchToWindowCommand { get; }
    }
}