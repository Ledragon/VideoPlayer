using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            }
            else if (e.Key == Key.Back)
            {
                this.MainGrid.RowDefinitions[1].Height = new GridLength(100);
                this.MainGrid.RowDefinitions[2].Height = new GridLength(80);
                this._uiVideosView.Visibility = Visibility.Hidden;
                this._uiSettingsView.Visibility = Visibility.Hidden;
            }
            else
            {
                if (this._uiVideosView.IsVisible)
                {
                    if (e.Key == Key.X)
                    {
                        this._uiVideosView._uiMediaElement.Stop();
                        e.Handled = true;
                    }
                    else if (e.Key == Key.Space)
                    {
                        if (this._uiVideosView._uiMediaElement.CanPause)
                        {
                            this._uiVideosView._uiMediaElement.Pause();
                        }
                        else
                        {
                            this._uiVideosView._uiMediaElement.Play();

                        }
                    }
                }
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
            backgroundWorker.RunWorkerAsync();
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this._uiVideosView.DataContext = this._videos;
            this._uiSettingsView.Directories = this._directories;
            this._uiSettingsView.DataContext = this._directories;
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //this._videos = this._controler.GetVideos();
            foreach (String file in this._controler.GetVideoFiles(@"D:\Users\Hugues\_Telechargements"))
            {
                Video video = new Video(file);
                this._videos.Add(video);
            }
            this._videos.OrderBy(i => i.FileName);
        }

        private void _uiSaveButton_Click(object sender, RoutedEventArgs e)
        {
            String filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ObjectsWrapper wrapper = new ObjectsWrapper();
            wrapper.Videos = this._videos;
            this._controler.Save(filePath, wrapper);
        }

        private void _uiLoadButton_Click(object sender, RoutedEventArgs e)
        {
            //this._videos.Clear();
            //foreach (Classes.Directory directory in this._directories)
            //{
            //    this._videos = this._controler.GetVideos(this._controler.GetVideoFiles(directory.DirectoryPath));
            //}
        }
    }
}
