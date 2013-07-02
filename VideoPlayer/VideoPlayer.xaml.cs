using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using Path = System.IO.Path;
using Classes;
using System.Collections.ObjectModel;
using Controlers;
using System.ComponentModel;

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Video> _videos = new ObservableCollection<Video>();
        private ObservableCollection<Classes.Directory> _directories = new ObservableCollection<Classes.Directory>();
        Controler _controler = new Controler();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                this.MainGrid.RowDefinitions[1].Height = new GridLength(100);
                this.MainGrid.RowDefinitions[2].Height = new GridLength(80);
                this._uiVideosView.Visibility = Visibility.Hidden;
                this._uiSettingsView.Visibility = Visibility.Hidden;
                e.Handled = true;

            }
            else if (e.Key == Key.Return && Keyboard.Modifiers == ModifierKeys.Alt)
            {
                this.WindowStyle = System.Windows.WindowStyle.None;
                e.Handled = true;

            }
            else if (e.Key == Key.S)
            {
                this.Close();
                e.Handled = true;

            }
        }

        private void _uiSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConfigureInterface();
            this._uiSettingsView.Visibility = System.Windows.Visibility.Visible;
            this._uiVideosView.Visibility = System.Windows.Visibility.Hidden;

        }

        private void _uiVideosButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConfigureInterface();
            this._uiSettingsView.Visibility = System.Windows.Visibility.Hidden;
            this._uiVideosView.Visibility = Visibility.Visible;
        }

        private void ConfigureInterface()
        {
            this.MainGrid.RowDefinitions[1].Height = new GridLength(0);
            this.MainGrid.RowDefinitions[2].Height = new GridLength(0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += this.backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            //this._uiCurrentOperationStatusBarItem.Content = "Loading library";
            backgroundWorker.RunWorkerAsync();
            //this._uiNumberOfVideosStatusBarItem.DataContext = this._videos;
            this._uiVideosView.DataContext = this._videos;
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

        private void _uiSaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Save();
            //Vlc.DotNet.Core.VlcContext.CloseAll();
        }

        private void Save()
        {
            ObjectsWrapper wrapper = new ObjectsWrapper();
            wrapper.Videos = this._videos;
            wrapper.Directories = this._directories;
            this._controler.Save(wrapper);
        }

        private void _uiLoadButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker backgroundWorkerLoad = new BackgroundWorker();
            backgroundWorkerLoad.DoWork += this.backgroundWorkerLoad_DoWork;
            backgroundWorkerLoad.RunWorkerCompleted += backgroundWorkerLoad_RunWorkerCompleted;
            backgroundWorkerLoad.RunWorkerAsync();
            //this._uiCurrentOperationStatusBarItem.Content = "Ready";
        }

        private void backgroundWorkerLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Finished loading");
            //this._uiCurrentOperationStatusBarItem.Content = "Ready";
        }

        private void backgroundWorkerLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            Action<Video> addMethod = (video) => this._videos.Add(video);
            Video[] tmpList = this._videos.ToArray();
            foreach (Classes.Directory directory in this._directories)
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

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            this.Save();
        }

        private void _uiCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Save();
            this.Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                this.MainGrid.RowDefinitions[1].Height = new GridLength(100);
                this.MainGrid.RowDefinitions[2].Height = new GridLength(80);
                this._uiVideosView.Visibility = Visibility.Hidden;
                this._uiSettingsView.Visibility = Visibility.Hidden;
            }
            else if (e.SystemKey == Key.Return && Keyboard.Modifiers == ModifierKeys.Alt)
            {
                if (this.WindowStyle == System.Windows.WindowStyle.None)
                {
                    this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                }
                else
                {
                    this.WindowStyle = System.Windows.WindowStyle.None;
                }
            }
        }

        private void _uiCleanButton_Click(object sender, RoutedEventArgs e)
        {
            List<String> existingFiles = new List<String>();
            foreach (Classes.Directory directory in this._directories)
            {
                List<String> files = this._controler.GetVideoFiles(directory);
                foreach (String file in files)
                {
                    existingFiles.Add(file);
                }
            }
            var i = from t in this._videos select t.FileName;
            List<String> l = i.ToList<String>();
            List<String> videosToRemove = l.Except(existingFiles).ToList<String>();
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
    }
}
