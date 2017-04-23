using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using ApplicationCommands = VideoPlayer.Infrastructure.ApplicationCommands;

namespace VideoPlayer.SettingsModule
{
    public class SettingsButtonViewModel : ISettingsButtonViewModel
    {
        public SettingsButtonViewModel()
        {
            this.NavigateCommand =
                new DelegateCommand(() => { ApplicationCommands.NavigateCommand.Execute(typeof(SettingsPage)); });
        }
        public ICommand NavigateCommand { get; }

    }
}