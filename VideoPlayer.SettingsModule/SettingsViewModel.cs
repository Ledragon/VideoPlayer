using System.Collections.ObjectModel;
using System.Windows.Input;
using Classes;
using Microsoft.Practices.Prism.Commands;
using VideoPlayer.Infrastructure.ViewFirst;
using VideoPlayer.Services;

namespace VideoPlayer.SettingsModule
{
    public class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        private ObservableCollection<Directory> _directories;
        private Directory _selectedItem;

        public SettingsViewModel(ILibraryService libraryService)
        {
            this.Directories = libraryService.GetObjectsFromFile().Directories;
            this.BrowseCommand = new DelegateCommand(this.Browse);
        }

        public ObservableCollection<Directory> Directories
        {
            get { return this._directories; }
            set
            {
                if (Equals(value, this._directories))
                {
                    return;
                }
                this._directories = value;
                this.OnPropertyChanged();
            }
        }

        public Directory SelectedItem

        {
            get { return this._selectedItem; }
            set
            {
                if (Equals(value, this._selectedItem))
                {
                    return;
                }
                this._selectedItem = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand BrowseCommand { get; }

        private void Browse()
        {
            var directoryBrowser = new DirectoryBrowser();
            if (directoryBrowser.ShowDialog() ?? false)
            {
                this.Directories.Add(directoryBrowser.Directory);
            }
        }

        private void DoubleClick()
        {
            var directoryBrowser = new DirectoryBrowser(this.SelectedItem);
            directoryBrowser.ShowDialog();
        }
    }
}