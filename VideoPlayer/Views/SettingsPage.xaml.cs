using System;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace VideoPlayer
{

    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        public ObservableCollection<Classes.Directory> Directories { get; set; }

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void _uiAddDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            DirectoryBrowser directoryBrowser = new DirectoryBrowser();
            if (directoryBrowser.ShowDialog() ?? false)
            {
                this.Directories.Add(directoryBrowser.Directory);
            }
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DirectoryBrowser directoryBrowser = new DirectoryBrowser(this._uiDirectoriesListBox.SelectedItem as Classes.Directory);
            directoryBrowser.ShowDialog();
            //if (!(Boolean)directoryBrowser.ShowDialog())
            //{
            //}
        }
    }
}
