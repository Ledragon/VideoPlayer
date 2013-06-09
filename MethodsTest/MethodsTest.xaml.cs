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
using VideoPlayer;

namespace MethodsTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<Video> _videos = new List<Video>();
        
        private void InfoViewer_Loaded(object sender, RoutedEventArgs e)
        {
            String[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                Video myVideo = new Video(args[1]);
                this._videos.Add(myVideo);
                this.DataContext = myVideo;
            }

        }
    }
}
