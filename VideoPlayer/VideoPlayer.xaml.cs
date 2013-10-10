using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Controllers;
using VideoPlayer.ViewModels;
using Path = System.IO.Path;
using Classes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Log;

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ViewModel _viewModel;
        private ObservableCollection<Video> _videos;
        private ObservableCollection<Directory> _directories;
        readonly Controller _controller = new Controller();

        public MainWindow()
        {
            InitializeComponent();
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this._uiVideosView.DataContext = this._videos;
            this._uiSettingsView.Directories = this._directories;
            this._uiSettingsView.DataContext = this._directories;
            //this._uiCurrentOperationStatusBarItem.Content = "Ready";

        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ViewModel viewModel = new ViewModel();
            this._viewModel = viewModel;
            this._videos = viewModel.VideoCollection;
            this._directories = viewModel.DirectoryCollection;
        }

        private void Save()
        {
            this._viewModel.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += this.backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();
            Logger.SetPath(Path.Combine(this._controller.GetDefaultFolder(), "Log.txt"));
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
            this.Save();
            Logger.Close();
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
