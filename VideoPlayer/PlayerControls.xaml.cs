using System;
using System.Collections.Generic;
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

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for PlayerControls.xaml
    /// </summary>
    public partial class PlayerControls : UserControl
    {
        private MediaElement _mediaElement;

        public PlayerControls()
        {
            InitializeComponent();
        }

        public PlayerControls(MediaElement mediaElement):this()
        {
            this._mediaElement = mediaElement;
        }

        private void _uiPlayButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void _uiPauseButton_Click(object sender, RoutedEventArgs e)
        {
            this._mediaElement.Pause();
        }

        private void _uiStopButton_Click(object sender, RoutedEventArgs e)
        {
            this._mediaElement.Stop();
        }

        private void _uiFullScreenButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void _uiMuteButton_Click(object sender, RoutedEventArgs e)
        {
            this._mediaElement.IsMuted = !this._mediaElement.IsMuted;
        }
    }
}
