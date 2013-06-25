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
using Vlc.DotNet.Core;
using Path = System.IO.Path;
using Vlc.DotNet.Core.Medias;

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
            // Set libvlc.dll and libvlccore.dll directory path
            String programFilesPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);
            VlcContext.LibVlcDllsPath = System.IO.Path.Combine(programFilesPath, @"VideoLan\VLC");

            // Set the vlc plugins directory path
            VlcContext.LibVlcPluginsPath = Path.Combine(VlcContext.LibVlcDllsPath,"plugins");

            /* Setting up the configuration of the VLC instance.
             * You can use any available command-line option using the AddOption function (see last two options). 
             * A list of options is available at 
             *     http://wiki.videolan.org/VLC_command-line_help
             * for example. */

            // Ignore the VLC configuration file
            VlcContext.StartupOptions.IgnoreConfig = true;

            // Enable file based logging
            //VlcContext.StartupOptions.LogOptions.LogInFile = true;

            // Set the log level for the VLC instance
            //VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.Debug;

            // Disable showing the movie file name as an overlay
            //VlcContext.StartupOptions.AddOption("--no-video-title-show");

            // Pauses the playback of a movie on the last frame
            //VlcContext.StartupOptions.AddOption("--play-and-pause");

            // Initialize the VlcContext
            VlcContext.Initialize();
            InitializeComponent();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.PlaySelectedVideo();
        }

        private void _uiMuteButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlc.audio.toggleMute();
            if (this._vlc.audio.mute)
            {
                this._uiMuteButton.Content = "Unmute";
            }
            else
            {
                this._uiMuteButton.Content = "Mute";
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X)
            {
                this._vlc.playlist.stop();
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
            this._vlc.playlist.add("file:///" + video.FileName, System.IO.Path.GetFileNameWithoutExtension(video.FileName), null);
            this._vlc.playlist.play();
            this._uiVLC.Media = new PathMedia(video.FileName);
            this._uiVLC.Play();
        }

        private void _uiFullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlc.video.toggleFullscreen();
            //this._vlc.Toolbar = true;
        }

        void VideosPage_MouseMove(object sender, MouseEventArgs e)
        {
            if (this._vlc.video.fullscreen)
            {
                //this._uiTestRectangle.Visibility = System.Windows.Visibility.Visible;
            }
            else 
            {
                this.MouseMove -= VideosPage_MouseMove;
                //this._uiTestRectangle.Visibility = System.Windows.Visibility.Hidden;

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this._uiFilesListBox.Items.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            this._vlc = new AxVLCPlugin2();
            //this.windowsFormHost.Child = this._vlc;
            this.MouseMove += VideosPage_MouseMove;
            this._vlc.audio.toggleMute();
            //this._vlc.Toolbar = false;
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
