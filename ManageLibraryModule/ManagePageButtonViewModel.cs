using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using VideoPlayer.Infrastructure.ViewFirst;
using ApplicationCommands = VideoPlayer.Infrastructure.ApplicationCommands;

namespace ManageLibraryModule
{
    public class ManagePageButtonViewModel : ViewModelBase, IManagePageButtonViewModel
    {
        public ManagePageButtonViewModel()
        {
            this.NavigateCommand = 
                new DelegateCommand(() => { ApplicationCommands.NavigateCommand.Execute(typeof (EditView)); });
        }
        public ICommand NavigateCommand { get; }
    }
}