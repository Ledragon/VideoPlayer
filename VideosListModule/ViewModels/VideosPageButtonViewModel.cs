using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using ApplicationCommands = VideoPlayer.Infrastructure.ApplicationCommands;

namespace VideosListModule.ViewModels
{
    public class VideosPageButtonViewModel
    {
        public VideosPageButtonViewModel()
        {
            this.GoToVideosCommand =
                new DelegateCommand(() => { ApplicationCommands.NavigateCommand.Execute(typeof(VideosListView)); });
        }

        private ICommand GoToVideosCommand { get; }
    }
}