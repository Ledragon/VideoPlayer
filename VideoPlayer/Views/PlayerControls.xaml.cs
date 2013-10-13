using Classes;
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
using Vlc.DotNet.Wpf;

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for PlayerControls.xaml
    /// </summary>
    public partial class PlayerControls : UserControl
    {
        private VlcControl _vlc;
        public Video currentVideo { get; set; }

        public PlayerControls()
        {
            InitializeComponent();
        }

        public PlayerControls(VlcControl vlc):this()
        {
            this._vlc = vlc;
        }

        private void _uiPlayButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlc.Play();            
        }

        private void _uiPauseButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlc.Pause();
        }

        private void _uiStopButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlc.Stop();
        }

        private void _uiFullScreenButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void _uiMuteButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlc.AudioProperties.IsMute = !this._vlc.AudioProperties.IsMute;
            if (this._vlc.AudioProperties.IsMute)
            {
                this._uiMuteButton.Content = "Unmute";
            }
            else
            {
                this._uiMuteButton.Content = "Mute";
            }
        }
    }
}
