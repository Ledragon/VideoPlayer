using Classes;
using System;
using System.Collections.Generic;
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
using Controlers;
using System.Collections.ObjectModel;
//using VideoPlayer;

namespace MethodsTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String _videosCount;
        public String VideosCount 
        {
            get { return this._videos.Count.ToString(); }
            set { this._videosCount = value;} 
        }
            Controler controler = new Controler();
        public MainWindow()
        {
            InitializeComponent();            
        }

        private ObservableCollection<Video> _videos = new ObservableCollection<Video>();
        
        private void InfoViewer_Loaded(object sender, RoutedEventArgs e)
        {
            String[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {

                foreach (String fil in this.controler.GetVideoFiles(args[1]))
                {
                    Video video = new Video(fil);
                    this._videos.Add(video);
                    this.FilesList.Items.Add(video.FileName);
                    this._uiVideosCount.Text = this._videos.Count.ToString();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ObjectsWrapper wrapper = new ObjectsWrapper();
            wrapper.Videos = this._videos;
            this.controler.Save(filePath, wrapper);
        }
    }
}
