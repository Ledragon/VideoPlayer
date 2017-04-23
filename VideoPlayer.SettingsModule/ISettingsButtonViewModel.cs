using System.Windows.Input;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideoPlayer.SettingsModule
{
    public interface ISettingsButtonViewModel : IViewModel
    {
        ICommand NavigateCommand { get; }
    }
}