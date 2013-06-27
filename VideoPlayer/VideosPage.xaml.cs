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
using System.Windows.Markup;
using System.Xml;
using AxAXVLC;

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
        private Boolean IsPositionChanging = false;
        private AxVLCPlugin2 _vlcActiveX;

        public VideosPage()
        {   
            #region LibVLC
		
            // Set libvlc.dll and libvlccore.dll directory path
            //String programFilesPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);
            //VlcContext.LibVlcDllsPath = System.IO.Path.Combine(programFilesPath, @"VideoLan\VLC");

            //// Set the vlc plugins directory path
            //VlcContext.LibVlcPluginsPath = Path.Combine(VlcContext.LibVlcDllsPath,"plugins");

            //// Ignore the VLC configuration file
            //VlcContext.StartupOptions.IgnoreConfig = true;

            //// Enable file based logging
            ////VlcContext.StartupOptions.LogOptions.LogInFile = true;

            //// Set the log level for the VLC instance
            ////VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.Debug;

            //// Disable showing the movie file name as an overlay
            //VlcContext.StartupOptions.AddOption("--no-video-title-show");

            //// Pauses the playback of a movie on the last frame
            ////VlcContext.StartupOptions.AddOption("--play-and-pause");

            //VlcContext.StartupOptions.ScreenSaverEnabled = false;

            //// Initialize the VlcContext
            //VlcContext.Initialize(); 
	        #endregion

            InitializeComponent();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.PlaySelectedVideo();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X)
            {
                //this._uiVLC.Stop();
                this._vlcActiveX.playlist.stop();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                if (this.IsFullScreenVideo)
                {
                    this.WindowMode();
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Return)
            {
                this.PlaySelectedVideo();
                e.Handled = true;
            }
        }

        private void WindowMode()
        {
            this.IsFullScreenVideo = false;
            this._uiVLCFullScrenGrid.Visibility = System.Windows.Visibility.Hidden;
            this.timer.Stop();
            this.Cursor = Cursors.Arrow;
        }

       private void _uiMuteButton_Click(object sender, RoutedEventArgs e)
        {
            //this._uiVLC.AudioProperties.IsMute = !this._uiVLC.AudioProperties.IsMute;
            this._vlcActiveX.audio.mute = !this._vlcActiveX.audio.mute;
            //if (this._uiVLC.AudioProperties.IsMute)
            if (this._vlcActiveX.audio.mute)
            {
                this._uiMuteButton.Content = "Unmute";
                this._uiFullScreenMuteButton.Content = "Unmute";
            }
            else
            {
                this._uiMuteButton.Content = "Mute";
                this._uiFullScreenMuteButton.Content = "Mute";
            }
        }

        private void _uiStopButton_Click(object sender, RoutedEventArgs e)
        {
            //this._uiVLC.Stop();
            this._vlcActiveX.playlist.stop();
            this._uiSlider.Value = 0;
            this._uiFullScreenSlider.Value = 0;
        }

        private void _uiPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this._vlcActiveX.playlist.isPlaying)
            {
                //this._uiVLC.Play();
                this._vlcActiveX.playlist.play();
            }
            else
            {
                //this._uiVLC.Pause();
                this._vlcActiveX.playlist.pause();
            }
        }

        private void _uiPlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this._vlcActiveX.playlist.isPlaying)
            {
                //this._uiVLC.Play();
                this._vlcActiveX.playlist.play();
            }
            else
            {
                this.PlaySelectedVideo();
            }
        }

        private void PlaySelectedVideo()
        {
            Video video = this._uiFilesListBox.SelectedItem as Video;
            if (this._vlcActiveX.playlist.isPlaying)
            {
                //this._uiVLC.Stop();
                this._vlcActiveX.playlist.stop();
                this._uiSlider.Value = 0;
                this._uiFullScreenSlider.Value = 0;
            }
            //if(this._uiVLC.Media!=null)
            //{
            //    this._uiVLC.Media.ParsedChanged -= Media_ParsedChanged;
            //}
            //this._uiVLC.Media = new PathMedia(video.FileName);
            //this._uiVLC.Media.ParsedChanged += Media_ParsedChanged;
            //this._uiVLC.Play();
            //this._uiVLC.Playing += _uiVLC_Playing;

            this._vlcActiveX.playlist.items.clear();
            this._vlcActiveX.playlist.add("file:///" + video.FileName, null, null);
            this._vlcActiveX.Toolbar = false;
            this._vlcActiveX.playlist.play();
            this.SwitchToFullScreen();
        }

        //void _uiVLC_Playing(Vlc.DotNet.Wpf.VlcControl sender, VlcEventArgs<EventArgs> e)
        //{
        //    Video video = this._uiFilesListBox.SelectedItem as Video;

        //    if (video.Source == null)
        //    {
        //        String temporaryFolderPath = System.Environment.GetEnvironmentVariable("TEMP");
        //        String temporaryImagePath = Path.Combine(temporaryFolderPath,"snapshot.png");
        //        //this._uiVLC.TakeSnapshot(temporaryImagePath, uint.Parse(this._uiVLC.VideoSource.Width.ToString("0")), uint.Parse(this._uiVLC.VideoSource.Height.ToString("0")));
        //        //BitmapImage image = new BitmapImage(new Uri(temporaryImagePath));
        //    }
        //}

        private void _uiFullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchToFullScreen();
        }

        private void SwitchToFullScreen()
        {
            this.IsFullScreenVideo = true;
            this._uiVLCFullScrenGrid.Visibility = System.Windows.Visibility.Visible;
            //this._vlcActiveX.video.fullscreen = false;
            //this._vlcActiveX.video.toggleFullscreen();
            this.timer.Start();
        }

        private void _uiWindowedButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowMode();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this._uiFilesListBox.Items.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            this.timer.Interval = 1000;
            this.timer.Tick += timer_Tick;
            this._vlcActiveX = new AxVLCPlugin2();
            this.tmp.Child = this._vlcActiveX;
            this._uiVlcHost.Child = new System.Windows.Forms.Button();
            this._uiVlcHost.Child.SendToBack();
            this._vlcActiveX.Toolbar = false;
            //this._uiVLC.PositionChanged += _uiVLC_PositionChanged;
            this._vlcActiveX.MediaPlayerPositionChanged += _vlcActiveX_MediaPlayerPositionChanged;
            //this._vlcActiveX.video.a = false;
        }

        void _vlcActiveX_MediaPlayerPositionChanged(object sender, DVLCEvents_MediaPlayerPositionChangedEvent e)
        {
            if (!this.IsPositionChanging)
            {
                this._uiSlider.Value = this._vlcActiveX.input.Position;
                this._uiFullScreenSlider.Value = this._vlcActiveX.input.Position;
            }
        }

        //void Media_ParsedChanged(MediaBase sender, VlcEventArgs<int> e)
        //{
        //    this._uiDuration.DataContext = this._vlcActiveX.input.;
        //    Video video = this._uiFilesListBox.SelectedItem as Video;
        //    if (TimeSpan.Compare(video.Length, TimeSpan.Zero) == 0)
        //    {
        //        video.Length = this._uiVLC.Media.Duration;
        //    }
        //}

        void _uiVLC_PositionChanged(Vlc.DotNet.Wpf.VlcControl sender, VlcEventArgs<float> e)
        {
            if (!this.IsPositionChanging)
            {
                this._uiSlider.Value = e.Data;
                this._uiFullScreenSlider.Value = e.Data;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now - this.mouseLastMouveDateTime > new TimeSpan(0, 0, 2))
            {
                this.Cursor = Cursors.None;
                this._uiFullScreenControlsGrid.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void _uiFasterButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlcActiveX.input.rate *= 2;
            //this._uiVLC.Rate *= 2;
        }

        private void _uiSlowerButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlcActiveX.input.rate /= 2; ;
            //this._uiVLC.Rate /= 2;

        }

        private void _uiVLCFullScrenGrid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsFullScreenVideo)
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
            this._uiFullScreenControlsGrid.Visibility = System.Windows.Visibility.Visible;
        }

        private void _uiFullScreenSlider_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //this._uiVLC.PositionChanged -= _uiVLC_PositionChanged;
            this._vlcActiveX.MediaPlayerPositionChanged -= _vlcActiveX_MediaPlayerPositionChanged;
            this.IsPositionChanging = true;
        }

        private void _uiFullScreenSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //this._uiVLC.Position = (float) this._uiFullScreenSlider.Value;
            //this._uiVLC.PositionChanged += _uiVLC_PositionChanged;
            this._vlcActiveX.input.Position = this._uiFullScreenSlider.Value;
            this._vlcActiveX.MediaPlayerPositionChanged += _vlcActiveX_MediaPlayerPositionChanged;
            this.IsPositionChanging = false;
        }

        private void _uiFullScreenSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            if (this.IsPositionChanging)
            {
                //this._uiVLC.Position = (float)e.NewValue;
                this._vlcActiveX.input.Position = e.NewValue;
            }
            
            //TODO
            //if (this._uiVLC.Media != null)
            //{
            //    Int32 seconds = Int32.Parse((this._uiFullScreenSlider.Value * this._uiVLC.Media.Duration.TotalSeconds).ToString("0"));
            //    TimeSpan position = new TimeSpan(0, 0, seconds);
            //    this._uiPosition.Text = position.ToString("hh\\:mm\\:ss");
            //}
        }

        private void _uiSnapshotButton_Click(object sender, RoutedEventArgs e)
        {
            //this._uiVLC.TakeSnapshot(@"D:\Users\Hugues\test.png", uint.Parse(this._uiVLC.VideoSource.Width.ToString("0")), uint.Parse(this._uiVLC.VideoSource.Height.ToString("0")));
        }

    }
}
