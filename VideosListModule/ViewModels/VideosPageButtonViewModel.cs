using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using VideoPlayer.Infrastructure.ViewFirst;
using VideosPageModule;
using ApplicationCommands = VideoPlayer.Infrastructure.ApplicationCommands;

namespace VideosListModule.ViewModels
{
    public class VideosPageButtonViewModel : IVideosPageButtonViewModel
    {
        public VideosPageButtonViewModel()
        {
            this.GoToVideosCommand =
                new DelegateCommand(() => { ApplicationCommands.NavigateCommand.Execute(typeof (VideosPage)); });
        }

        public ICommand GoToVideosCommand { get; }
    }

    public interface IVideosPageButtonViewModel : IViewModel
    {
        ICommand GoToVideosCommand { get; }
    }
}