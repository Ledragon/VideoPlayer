using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Classes;
using Log;
using VideoPlayer.Helpers;
using VideoPlayer.ViewModels;
using Directory = Classes.Directory;

namespace VideoPlayer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ObservableCollection<Directory> _directories;
        private ObservableCollection<Video> _videos;
        private ViewModel _viewModel;

        public MainWindow()
        {
            this.InitializeComponent();
        }

        public ObservableCollection<Video> Videos
        {
            get { return this._videos; }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this._uiVideosView.VideoCollection = this._videos;
            this._uiSettingsView.Directories = this._directories;
            this._uiSettingsView.DataContext = this._directories;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var viewModel = new ViewModel();
            this._viewModel = viewModel;
            this._videos = viewModel.VideoCollection;
            this._directories = viewModel.DirectoryCollection;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            new LoggingSystemManager().SetPath(Path.Combine(FileSystemHelper.GetDefaultFolder(), "Log.txt"));
            Bootstrapper.Bootstrapper.BuildContainer();
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += this.backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += this.backgroundWorker_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.OriginalSource is TextBox))
            {
                if (e.Key == Key.Escape)
                {
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    e.Handled = true;
                }
                else if (e.Key == Key.Back)
                {
                    this._uiTabs.SelectedItem = this._uiHomeTab;
                    e.Handled = true;
                }
                else if (e.Key == Key.Return && Keyboard.Modifiers == ModifierKeys.Alt)
                {
                    this.WindowStyle = WindowStyle.None;
                    e.Handled = true;
                }
                else if (e.Key == Key.S)
                {
                    MessageBoxResult mbr = MessageBox.Show("Do you really want to exit?", "Exit?",
                        MessageBoxButton.YesNo);
                    if (mbr == MessageBoxResult.Yes)
                    {
                        this.Close();
                    }
                    e.Handled = true;
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            this._viewModel.Save();
        }

        private void _uiHomePage_VideoClick(object sender, RoutedEventArgs e)
        {
            this._uiTabs.SelectedItem = this._uiVideoTab;
        }

        private void _uiHomePage_CleanClick(object sender, RoutedEventArgs e)
        {
            this._viewModel.Clean();
        }

        private void _uiHomePage_SettingsClick(object sender, RoutedEventArgs e)
        {
            this._uiTabs.SelectedItem = this._uiSettingsTab;
            //if (this._uiSettingsTab.Content == null)
            //{
            //    SettingsPage settingsPage = new SettingsPage();
            //    settingsPage.HorizontalAlignment = HorizontalAlignment.Stretch;
            //    settingsPage.VerticalAlignment = VerticalAlignment.Stretch;
            //    settingsPage.Name = "_uiVideosView";
            //    settingsPage.Directories = this._directories;
            //    settingsPage.DataContext = this._directories;
            //    this._uiSettingsTab.Content = settingsPage;
            //}
        }

        private void _uiHomePage_LoadClick(object sender, RoutedEventArgs e)
        {
            this._viewModel.Load(this.Dispatcher);
        }
    }
}