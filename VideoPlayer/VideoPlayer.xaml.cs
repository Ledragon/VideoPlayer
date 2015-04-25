using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HomeModule;
using Log;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Common;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;

namespace VideoPlayer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private IEventAggregator _eventAggregator;
        private ILibraryService _libraryService;

        public MainWindow()
        {
            this.InitializeComponent();
            try
            {
                this.DataContext = DependencyFactory.Resolve<IVideoPlayerViewModel>();
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this._libraryService = DependencyFactory.Resolve<ILibraryService>();
            //this._libraryService.Update();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += this.backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += this.backgroundWorker_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();

            this._eventAggregator = DependencyFactory.Resolve<IEventAggregator>();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.OriginalSource is TextBox))
            {
                if (e.Key == Key.S)
                {
                    var mbr = MessageBox.Show("Do you really want to exit?", "Exit?",
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
            this._libraryService.Save();
        }

        //private void _uiHomePage_VideoClick(object sender, RoutedEventArgs e)
        //{
        //    this._uiTabs.SelectedItem = this._uiVideoTab;
        //}

        private void _uiHomePage_CleanClick(object sender, RoutedEventArgs e)
        {
            this._libraryService.Clean();
            this._eventAggregator.GetEvent<LibraryUpdated>().Publish(null);
        }

        //private void _uiHomePage_SettingsClick(object sender, RoutedEventArgs e)
        //{
        //    this._uiTabs.SelectedItem = this._uiSettingsTab;
        //}

        private void _uiHomePage_LoadClick(object sender, RoutedEventArgs e)
        {
            this.LoadingTextBlock.Visibility = Visibility.Visible;
            this._libraryService.Update();
            this._eventAggregator.GetEvent<LibraryUpdated>().Publish(null);
            this.LoadingTextBlock.Visibility = Visibility.Hidden;
        }
    }
}