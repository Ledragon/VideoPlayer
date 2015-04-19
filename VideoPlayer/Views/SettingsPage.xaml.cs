using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Classes;
using VideoPlayer.Common;
using VideoPlayer.Services;

//using System.Threading.Tasks;

namespace VideoPlayer
{
    /// <summary>
    ///     Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            var libraryService = DependencyFactory.Resolve<ILibraryService>();
            this.Directories = libraryService.GetObjectsFromFile().Directories;
            this.DataContext = this.Directories;
        }

        public ObservableCollection<Directory> Directories { get; set; }

        private void _uiAddDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            var directoryBrowser = new DirectoryBrowser();
            if (directoryBrowser.ShowDialog() ?? false)
            {
                this.Directories.Add(directoryBrowser.Directory);
            }
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var directoryBrowser = new DirectoryBrowser(this._uiDirectoriesListBox.SelectedItem as Directory);
            directoryBrowser.ShowDialog();
            //if (!(Boolean)directoryBrowser.ShowDialog())
            //{
            //}
        }
    }
}