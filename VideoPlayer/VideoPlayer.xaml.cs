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
            FilesList.ItemsSource = this._videos;
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (String file in this.controler.GetVideoFiles(this._uiDirectoryTextBox.Text))
            {
                Video video = new Video(file);
                this._videos.Add(video);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            String filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ObjectsWrapper wrapper = new ObjectsWrapper();
            wrapper.Videos = this._videos;
            this.controler.Save(filePath, wrapper);
            this.Close();
        }
    }
}
