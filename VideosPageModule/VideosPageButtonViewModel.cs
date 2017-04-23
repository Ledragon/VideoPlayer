using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using ApplicationCommands = VideoPlayer.Infrastructure.ApplicationCommands;

namespace VideosPageModule
{
    public class VideosPageButtonViewModel : IVideosPageButtonViewModel
    {
        public VideosPageButtonViewModel()
        {
            this.NavigateCommand =
                new DelegateCommand(() => { ApplicationCommands.NavigateCommand.Execute(typeof (VideosPage)); });
        }

        public ICommand NavigateCommand { get; }
    }
}