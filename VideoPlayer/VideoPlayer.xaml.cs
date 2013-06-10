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

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Video> _videos = new ObservableCollection<Video>();
        Controler controler = new Controler();

        public MainWindow()
        {
            InitializeComponent();

            //TODO supprimer le code du constructeur
            //FilesList.ItemsSource = this._videos;
            //this.MainGrid.ColumnDefinitions[0].Width = new GridLength(0);
            foreach (String file in this.controler.GetVideoFiles(@"D:\Users\Hugues\_Telechargements"))
            {
                Video video = new Video(file);
                this._videos.Add(video);
            }
            this._videos.OrderBy(i => i.FileName);
            this.MainGrid.DataContext = this._videos;
        }

        //private void LoadButton_Click(object sender, RoutedEventArgs e)
        //{
        //    foreach (String file in this.controler.GetVideoFiles(this._uiDirectoryTextBox.Text))
        //    {
        //        Video video = new Video(file);
        //        this._videos.Add(video);
        //    }
        //    this._videos.OrderBy(i => i.FileName);
        //}

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            String filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ObjectsWrapper wrapper = new ObjectsWrapper();
            wrapper.Videos = this._videos;
            this.controler.Save(filePath, wrapper);
            this.Close();
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
            }
        }

        private void _uiSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConfigureInterface();

        }

        private void _uiVideosButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConfigureInterface();
            this._uiVideosView.Visibility = Visibility.Visible;
        }

        private void ConfigureInterface()
        {
            this.MainGrid.RowDefinitions[1].Height = new GridLength(0);
            this.MainGrid.RowDefinitions[2].Height = new GridLength(0);
        }
    }
}
