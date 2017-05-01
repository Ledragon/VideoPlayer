using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using ApplicationCommands = VideoPlayer.Infrastructure.ApplicationCommands;

namespace VideoPlayer.PlaylistManagement
{
    public class VideosPageButtonViewModel : IVideosPageButtonViewModel
    {
        public VideosPageButtonViewModel()
        {
            this.NavigateCommand =
                new DelegateCommand(() => { ApplicationCommands.NavigateCommand.Execute(typeof (PlayListManagementView)); });
        }

        public ICommand NavigateCommand { get; }
    }
}