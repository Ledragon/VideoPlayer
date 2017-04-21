using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using VideosPageModule;
using ApplicationCommands = VideoPlayer.Infrastructure.ApplicationCommands;

namespace VideosListModule.ViewModels
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