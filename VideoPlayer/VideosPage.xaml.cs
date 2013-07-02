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
using Vlc.DotNet.Wpf;


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

        public VideosPage()
        {   
            // Set libvlc.dll and libvlccore.dll directory path
            String programFilesPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);
            VlcContext.LibVlcDllsPath = System.IO.Path.Combine(programFilesPath, @"VideoLan\VLC");

            // Set the vlc plugins directory path
            VlcContext.LibVlcPluginsPath = Path.Combine(VlcContext.LibVlcDllsPath,"plugins");

            // refer to http://wiki.videolan.org/VLC_command-line_help for more information
            VlcContext.StartupOptions.IgnoreConfig = true;
            VlcContext.StartupOptions.AddOption("--no-video-title-show");
            VlcContext.StartupOptions.ScreenSaverEnabled = false;
            VlcContext.StartupOptions.AddOption("--no-snapshot-preview");
            VlcContext.StartupOptions.AddOption("--no-mouse-events");
            VlcContext.StartupOptions.AddOption("--no-keyboard-events");
            //VlcContext.StartupOptions.AddOption("--no-skip-frames");
            //VlcContext.StartupOptions.AddOption("--directx-hw-yuv");
            //VlcContext.StartupOptions.AddOption("--directx-3buffering");
            VlcContext.StartupOptions.AddOption("--ffmpeg-skiploopfilter=4");
            //VlcContext.StartupOptions.AddOption("--ffmpeg-hw");
            //VlcContext.StartupOptions.AddOption("--vout=directx");
            

            //VlcContext.StartupOptions.ShowLoggerConsole = false;
            //VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.Debug;
            //VlcContext.StartupOptions.LogOptions.LogInFile = false;
            //VlcContext.StartupOptions.LogOptions.LogInFilePath = @"D:\videoplayer_vlc.log";
            
            // Initialize the VlcContext
            VlcContext.Initialize();
            InitializeComponent();
            VlcControl control = new VlcControl();
            
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.PlaySelectedVideo();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X)
            {
                StopVideoPlaying();
                e.Handled = true;
            }
            else if (e.Key == Key.Tab)
            {
                if (this.IsFullScreenVideo)
                {
                    this.WindowMode();
                }
                else
                {
                    this.SwitchToFullScreen();
                }
                e.Handled = true;

            }
            else if (e.Key == Key.Return)
            {
                this.PlaySelectedVideo();
                e.Handled = true;
            }
        }

        private void StopVideoPlaying()
        {
            this._uiVLC.Stop();
            this._uiFullScreenSlider.Value = 0;
            this._uiNowPlaying.Text = "Now playing: ";
            this.WindowMode();
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
            this._uiVLC.AudioProperties.IsMute = !this._uiVLC.AudioProperties.IsMute;
            if (this._uiVLC.AudioProperties.IsMute)
            {
                this._uiFullScreenMuteButton.Content = FindResource("Muted");
            }
            else
            {
                this._uiFullScreenMuteButton.Content = FindResource("Unmuted");
            }
        }

        private void _uiStopButton_Click(object sender, RoutedEventArgs e)
        {
            this.StopVideoPlaying();
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
                this._uiFullScreenSlider.Value = 0;
            }
            if(this._uiVLC.Media!=null)
            {
                this._uiVLC.Media.ParsedChanged -= Media_ParsedChanged;
            }
            this._uiVLC.Media = new PathMedia(video.FileName);
            this._uiVLC.Media.ParsedChanged += Media_ParsedChanged;
            this._uiNowPlaying.Text = "Now playing: " + video.Title;
            this.SwitchToFullScreen();
            this._uiVLC.Play();
        }

        private void _uiFullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchToFullScreen();
        }

        private void SwitchToFullScreen()
        {
            this.IsFullScreenVideo = true;
            this._uiVLCFullScrenGrid.Visibility = System.Windows.Visibility.Visible;
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
            this._uiVLC.PositionChanged += _uiVLC_PositionChanged;
        }

        void Media_ParsedChanged(MediaBase sender, VlcEventArgs<int> e)
        {
            this._uiDuration.DataContext = this._uiVLC.Media;
            Video video = this._uiFilesListBox.SelectedItem as Video;
            if (TimeSpan.Compare(video.Length, TimeSpan.Zero) == 0)
            {
                video.Length = this._uiVLC.Media.Duration;
            }
        }

        void _uiVLC_PositionChanged(Vlc.DotNet.Wpf.VlcControl sender, VlcEventArgs<float> e)
        {
            if (!this.IsPositionChanging)
            {
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
            this._uiVLC.Rate *= 2;
        }

        private void _uiSlowerButton_Click(object sender, RoutedEventArgs e)
        {
            this._uiVLC.Rate /= 2;

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
            this._uiVLC.PositionChanged -= _uiVLC_PositionChanged;
            this.IsPositionChanging = true;
        }

        private void _uiFullScreenSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this._uiVLC.Position = (float) this._uiFullScreenSlider.Value;
            this._uiVLC.PositionChanged += _uiVLC_PositionChanged;
            this.IsPositionChanging = false;
        }

        private void _uiFullScreenSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.IsPositionChanging)
            {
                this._uiVLC.Position = (float)e.NewValue;
            }
            if (this._uiVLC.Media != null)
            {
                Int32 seconds = Int32.Parse((this._uiFullScreenSlider.Value * this._uiVLC.Media.Duration.TotalSeconds).ToString("0"));
                TimeSpan position = new TimeSpan(0, 0, seconds);
                this._uiPosition.Text = position.ToString("hh\\:mm\\:ss");
            }
        }

        private void _uiSnapshotButton_Click(object sender, RoutedEventArgs e)
        {
            this._uiVLC.TakeSnapshot(@"D:\Users\Hugues\test.png", uint.Parse(this._uiVLC.VideoSource.Width.ToString("0")), uint.Parse(this._uiVLC.VideoSource.Height.ToString("0")));
        }

        private void _uiFullScreenSlider_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            this._uiVLC.PositionChanged -= _uiVLC_PositionChanged;
            this.IsPositionChanging = true;

            if (e.Delta < 0)
            {
                this._uiFullScreenSlider.Value -= this._uiFullScreenSlider.SmallChange;
            }
            else
            {
                this._uiFullScreenSlider.Value += this._uiFullScreenSlider.SmallChange;
            }

            this._uiVLC.PositionChanged += _uiVLC_PositionChanged;
            this.IsPositionChanging = false;
        }
    }
}
