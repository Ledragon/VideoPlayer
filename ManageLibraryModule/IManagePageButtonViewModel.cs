using System.Windows.Input;
using VideoPlayer.Infrastructure.ViewFirst;

namespace ManageLibraryModule
{
    public interface IManagePageButtonViewModel : IViewModel
    {
        ICommand NavigateCommand { get; }
    }
}