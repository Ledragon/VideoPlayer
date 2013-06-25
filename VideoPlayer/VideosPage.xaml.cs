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
        private Boolean IsFullScreenVideo = false;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private DateTime mouseLastMouveDateTime = DateTime.Now;

        public VideosPage()
        {   
            // Set libvlc.dll and libvlccore.dll directory path
            String programFilesPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);
            VlcContext.LibVlcDllsPath = System.IO.Path.Combine(programFilesPath, @"VideoLan\VLC");

            // Set the vlc plugins directory path
            VlcContext.LibVlcPluginsPath = Path.Combine(VlcContext.LibVlcDllsPath,"plugins");

            // Ignore the VLC configuration file
            VlcContext.StartupOptions.IgnoreConfig = true;

            // Enable file based logging
            //VlcContext.StartupOptions.LogOptions.LogInFile = true;

            // Set the log level for the VLC instance
            //VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.Debug;

            // Disable showing the movie file name as an overlay
            VlcContext.StartupOptions.AddOption("--no-video-title-show");

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
            this._uiVLC.AudioProperties.IsMute = !this._uiVLC.AudioProperties.IsMute;
            if (this._uiVLC.AudioProperties.IsMute)
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
                this._uiVLC.Stop();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                if (this.IsFullScreenVideo)
                {
                    this.IsFullScreenVideo = false;
                    this._uiVLCFullScrenGrid.Visibility = System.Windows.Visibility.Hidden;
                    this.timer.Stop();
                    
                    e.Handled = true;
                }
            }
        }

        private void _uiStopButton_Click(object sender, RoutedEventArgs e)
        {
            this._uiVLC.Stop();
        }

        private void _uiPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._uiVLC.IsPaused)
            {
                this._uiVLC.Play();
            }
            else
            {
                this._uiVLC.Pause();
            }
        }

        private void _uiPlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._uiVLC.IsPaused)
            {
                this._uiVLC.Play();
            }
            else
            {
                this.PlaySelectedVideo();
            }
        }

        private void PlaySelectedVideo()
        {
            Video video = this._uiFilesListBox.SelectedItem as Video;
            if (this._uiVLC.IsPlaying)
            {
                this._uiVLC.Stop();
            }
            this._uiVLC.Media = new PathMedia(video.FileName);
            this._uiVLC.Play();
        }

        private void _uiFullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            this._uiVLCFullScrenGrid.Visibility = System.Windows.Visibility.Visible;
            this.IsFullScreenVideo = true;
            this.timer.Start();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this._uiFilesListBox.Items.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            this.timer.Interval = 1000;
            this.timer.Tick += timer_Tick;
            this._uiVLC.PositionChanged += _uiVLC_PositionChanged;
            //TODO duplicate buttons in an appropriate control to show the during full screen play
            String test = System.Windows.Markup.XamlWriter.Save(this._uiSlider);
        }

        void _uiVLC_PositionChanged(Vlc.DotNet.Wpf.VlcControl sender, VlcEventArgs<float> e)
        {
            this._uiSlider.Value = e.Data;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now - this.mouseLastMouveDateTime > new TimeSpan(0, 0, 2))
            {
                this.Cursor = Cursors.None;
                //TODO implement the controls toolbar to display (see how to use the controls already available)
            }
        }

        private void _uiFasterButton_Click(object sender, RoutedEventArgs e)
        {
            //this._vlc.input.rate *= 2;
        }

        private void _uiSlowerButton_Click(object sender, RoutedEventArgs e)
        {
            //this._vlc.input.rate /= 2; ;
        }

        private void _uiVLCFullScrenGrid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this._uiVLCFullScrenGrid.IsVisible)
            {
                this._uiVLCFullScrenGrid.MouseMove += _uiVLCFullScrenGrid_MouseMove;
            }
            else
            {
                this._uiVLCFullScrenGrid.MouseMove -= _uiVLCFullScrenGrid_MouseMove;
            }
        }

        void _uiVLCFullScrenGrid_MouseMove(object sender, MouseEventArgs e)
        {
            this.mouseLastMouveDateTime = DateTime.Now;
            this.Cursor = Cursors.Arrow;            
        }
    }
}
