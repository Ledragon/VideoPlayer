using Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using AxAXVLC;

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for VideosPage.xaml
    /// </summary>
    public partial class VideosPage : UserControl
    {
        AxVLCPlugin2 _vlc;
        private Boolean IsFullScreenVideo = false;
        private GridLength _originalVideosListRow;
        private GridLength _videoInfosRow;
        private GridLength _videoPropertiesColumn;
        private GridLength _mediaElementColumn;

        public VideosPage()
        {
            InitializeComponent();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.PlaySelectedVideo();
        }

        private void _uiMuteButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlc.audio.toggleMute();
            //if (this._vlc.
            //{
            //    this._uiMuteButton.Content = "Unmute";
            //}
            //else
            //{
            //    this._uiMuteButton.Content = "Mute";
            //}
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X)
            {
                this._vlc.playlist.stop();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape && this.IsFullScreenVideo)
            {
                this.SetViewWindowed();
                e.Handled = true;
            }
            else if (e.Key == Key.F)
            {
                this.SetViewFullScreen();
                e.Handled = true; 
            }
        }

        private void _uiStopButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlc.playlist.stop();
        }

        private void _uiPauseButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlc.playlist.togglePause();
        
        }

        private void _uiPlayButton_Click(object sender, RoutedEventArgs e)
        {
            // State 4: Paused
            if (this._vlc.input.state == 4)
            {
                this._vlc.playlist.play();
            }
            else
            {
                this.PlaySelectedVideo();
            }
        }

        private void PlaySelectedVideo()
        {
            Video video = this._uiFilesListBox.SelectedItem as Video;
                this._vlc.playlist.items.clear();
            if (this._vlc.input.state == 3)
            {
                this._vlc.playlist.stop();
            }
            this._vlc.playlist.add("file:///" + video.FileName, System.IO.Path.GetFileName(video.FileName), null);
            this._vlc.playlist.play();
        }

        private void _uiFullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlc.video.toggleFullscreen();
            this._vlc.Toolbar = true;
        }

        private void SetViewFullScreen()
        {
            //Save original settings
            this._originalVideosListRow = new GridLength(this._uiVideosListRow.Height.Value, this._uiVideosListRow.Height.GridUnitType);
            this._videoInfosRow = new GridLength(this._uiVideoInfosRow.Height.Value, this._uiVideoInfosRow.Height.GridUnitType);
            this._videoPropertiesColumn = new GridLength(this._uiVideoPropertiesColumn.Width.Value, this._uiVideoPropertiesColumn.Width.GridUnitType);
            this._mediaElementColumn = new GridLength(this._uiMediaElementColumn.Width.Value, this._uiMediaElementColumn.Width.GridUnitType);

            this._uiVideosListRow.Height = new GridLength(0);
            this._uiVideoInfosRow.Height = new GridLength(100, GridUnitType.Star);
            this._uiVideoPropertiesColumn.Width = new GridLength(0);
            this._uiMediaElementColumn.Width = new GridLength(100, GridUnitType.Star);

            this._uiFullScreenButton.Content = "Windowed";
            this.IsFullScreenVideo = true;
        }

        private void SetViewWindowed()
        {
            this._uiVideosListRow.Height = this._originalVideosListRow;
            this._uiVideoInfosRow.Height = this._videoInfosRow;
            this._uiVideoPropertiesColumn.Width = this._videoPropertiesColumn;
            this._uiMediaElementColumn.Width = this._mediaElementColumn;
            this._uiFullScreenButton.Content = "Full";
            this.IsFullScreenVideo = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this._uiFilesListBox.Items.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            this._vlc = new AxVLCPlugin2();
            this.windowsFormHost.Child = this._vlc;
            this._vlc.Toolbar = false;
        }

        private void _uiFasterButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlc.input.rate *= 2;
        }

        private void _uiSlowerButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlc.input.rate /= 2; ;
        }
    }
}
