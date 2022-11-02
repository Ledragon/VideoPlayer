using System.Collections.ObjectModel;
using System.Windows.Input;
using VideoPlayer.Entities;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideoPlayer.SettingsModule
{
    public interface ISettingsViewModel : IViewModel
    {
        ICommand BrowseCommand { get; }
        ObservableCollection<Directory> Directories { get; set; }
        Directory SelectedItem { get; set; }
    }
}