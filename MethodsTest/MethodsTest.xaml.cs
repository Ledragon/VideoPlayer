using Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
//using VideoPlayer;
using Controllers;

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

        //readonly Controller _controller = new Controller();
        public MainWindow()
        {
            InitializeComponent();            
        }

        private readonly ObservableCollection<Video> _videos = new ObservableCollection<Video>();
        
        private void InfoViewer_Loaded(object sender, RoutedEventArgs e)
        {
            String[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {

                //foreach (String fil in this._controller.GetVideoFiles(args[1]))
                //{
                //    Video video = new Video(fil);
                //    this._videos.Add(video);
                //    this.FilesList.Items.Add(video.FileName);
                //    this._uiVideosCount.Text = this._videos.Count.ToString();
                //}
            }
            this._me.LoadedBehavior = MediaState.Manual;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //String filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ObjectsWrapper wrapper = new ObjectsWrapper();
            wrapper.Videos = this._videos;
            //this._controller.Save(filePath, wrapper);
        }

        private void _uiPlayButton_Click(object sender, RoutedEventArgs e)
        {
            this._me.Source = new Uri(@"D:\Users\Hugues\My Videos\MVI_1687.avi");
            this._me.Play();
        }

        private void _me_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            string s = e.ErrorException.Message;
            MessageBox.Show(s);
        }
    }
}
