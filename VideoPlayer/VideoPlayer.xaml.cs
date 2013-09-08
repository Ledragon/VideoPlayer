using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Path = System.IO.Path;
using Classes;
using System.Collections.ObjectModel;
using Controlers;
using System.ComponentModel;
using Log;

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ObservableCollection<Video> _videos = new ObservableCollection<Video>();
        private ObservableCollection<Directory> _directories = new ObservableCollection<Directory>();
        readonly Controler _controler = new Controler();

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
            ObjectsWrapper wrapper = this._controler.GetObjectsFromFile();
            if (wrapper != null)
            {
                this._videos = wrapper.Videos;
                this._directories = wrapper.Directories;
            }
        }

        private void Save()
        {
            ObjectsWrapper wrapper = new ObjectsWrapper();
            wrapper.Videos = this._videos;
            wrapper.Directories = this._directories;
            this._controler.Save(wrapper);
        }


        private void backgroundWorkerLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Finished loading");
            //this._uiCurrentOperationStatusBarItem.Content = "Ready";
        }

        private void backgroundWorkerLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            Action<Video> addMethod = video => this._videos.Add(video);
            Video[] tmpList = this._videos.ToArray();
            foreach (var directory in this._directories)
            {
                List<String> files = this._controler.GetVideoFiles(directory);
                foreach (String videoFile in files)
                {
                    if (!tmpList.Any(s => s.FileName == videoFile))
                    {
                        Video newVideo = new Video(videoFile);
                        // cross-thread
                        this.Dispatcher.BeginInvoke(addMethod, newVideo);
                        newVideo.DateAdded = DateTime.Now;
                    }
                }
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += this.backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();
            Logger.SetPath(Path.Combine(this._controler.GetDefaultFolder(), "Log.txt"));
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Back)
            //{
            //    this.MainGrid.RowDefinitions[1].Height = new GridLength(100);
            //    this.MainGrid.RowDefinitions[2].Height = new GridLength(80);
            //    this._uiVideosView.Visibility = Visibility.Hidden;
            //    this._uiSettingsView.Visibility = Visibility.Hidden;
            //}
            //if (e.SystemKey == Key.Return && Keyboard.Modifiers == ModifierKeys.Alt)
            //{
            //    if (this.WindowStyle == System.Windows.WindowStyle.None)
            //    {
            //        this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            //    }
            //    else
            //    {
            //        this.WindowStyle = System.Windows.WindowStyle.None;
            //    }
            //}
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
                    //this.MainGrid.RowDefinitions[1].Height = new GridLength(100);
                    //this.MainGrid.RowDefinitions[2].Height = new GridLength(80);
                    this._uiHomePage.Visibility = Visibility.Visible;
                    this._uiVideosView.Visibility = Visibility.Hidden;
                    this._uiSettingsView.Visibility = Visibility.Hidden;
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
            this._uiSettingsView.Visibility = Visibility.Hidden;
            this._uiVideosView.Visibility = Visibility.Visible;
            this._uiHomePage.Visibility = Visibility.Hidden;
        }

        private void _uiHomePage_CleanClick(object sender, RoutedEventArgs e)
        {
            List<String> existingFiles = new List<String>();
            foreach (var directory in this._directories)
            {
                List<String> files = this._controler.GetVideoFiles(directory);
                foreach (String file in files)
                {
                    existingFiles.Add(file);
                }
            }
            var i = from t in this._videos select t.FileName;
            List<String> l = i.ToList();
            List<String> videosToRemove = l.Except(existingFiles).ToList();
            foreach (String file in videosToRemove)
            {
                foreach (var video in this._videos)
                {
                    if (video.FileName == file)
                    {
                        this._videos.Remove(video);
                        break;
                    }
                }
            }

        }

        private void _uiHomePage_SettingsClick(object sender, RoutedEventArgs e)
        {
            this._uiHomePage.Visibility = Visibility.Hidden;
            this._uiSettingsView.Visibility = Visibility.Visible;
            this._uiVideosView.Visibility = Visibility.Hidden;
        }

        private void _uiHomePage_LoadClick(object sender, RoutedEventArgs e)
        {
            BackgroundWorker backgroundWorkerLoad = new BackgroundWorker();
            backgroundWorkerLoad.DoWork += this.backgroundWorkerLoad_DoWork;
            backgroundWorkerLoad.RunWorkerCompleted += backgroundWorkerLoad_RunWorkerCompleted;
            backgroundWorkerLoad.RunWorkerAsync();

        }

    }
}
