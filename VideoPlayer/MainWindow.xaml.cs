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

namespace VideoPlayer
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Video video = new Video();
            video.Directory = "directory";
            video.FileName = "filename";
            video.Length = new TimeSpan(0, 10, 30);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Video));
            using (StreamWriter wr = new StreamWriter("library.xml"))
            {
                xmlSerializer.Serialize(wr, video);
            }

        }
    }
}
