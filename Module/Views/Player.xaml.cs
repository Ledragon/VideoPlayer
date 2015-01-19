using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Classes;
using Log;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Common;
using VideoPlayer.Infrastructure;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Medias;
using Vlc.DotNet.Wpf;
using Image = System.Drawing.Image;

namespace Module
{
    /// <summary>
    ///     Interaction logic for Player.xaml
    /// </summary>
    public partial class Player : UserControl, IPlayer
    {
        public delegate void StoppedEventHandler(Object sender, EventArgs e);

        private readonly IEventAggregator _eventAggregator;
        private PlayerViewModel _viewModel;

        public Player()
        {
            this.InitializeComponent();
            this.InitializeVlc();
            this._eventAggregator = DependencyFactory.Resolve<IEventAggregator>();

            this._eventAggregator.GetEvent<StoppedEvent>().Subscribe(this.Stop);
            this._eventAggregator.GetEvent<RateChanged>().Subscribe(this.RateChanged);
            this._eventAggregator.GetEvent<PlayedEvent>().Subscribe(this.Play);
            this._eventAggregator.GetEvent<VideoPositionChanged>().Subscribe(this.PositionChanged);
        }

        private void PositionChanged(Single position)
        {
            this._VLCcontrol.Position = position;
        }

        public IViewModel ViewModel
        {
            get { return this.DataContext as IViewModel; }
            set
            {
                this.DataContext = value;
                this._viewModel = (PlayerViewModel) value;
            }
        }

        private void RateChanged(Single rate)
        {
            this._VLCcontrol.Rate = rate;
        }

        private void Play(Video video)
        {
            this.ExecuteHandled(() =>
            {
                if (video != null)
                {
                    this.Logger().DebugFormat("Playing video '{0}'.", video.Title);
                    video.NumberOfViews++;
                    this._VLCcontrol.Media = new PathMedia(video.FileName);
                }
            });
        }

        private void InitializeVlc()
        {
            String programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            VlcContext.LibVlcDllsPath = Path.Combine(programFilesPath, @"VideoLan\VLC");

            // Set the vlc plugins directory path
            VlcContext.LibVlcPluginsPath = Path.Combine(VlcContext.LibVlcDllsPath, "plugins");

            // refer to http://wiki.videolan.org/VLC_command-line_help for more information
            VlcContext.StartupOptions.IgnoreConfig = true;
            VlcContext.StartupOptions.AddOption("--no-video-title-show");
            VlcContext.StartupOptions.ScreenSaverEnabled = false;
            VlcContext.StartupOptions.AddOption("--no-snapshot-preview");
            VlcContext.StartupOptions.AddOption("--no-mouse-events");
            VlcContext.StartupOptions.AddOption("--no-keyboard-events");
            VlcContext.StartupOptions.AddOption("--disable-screensaver");
            // Initialize the VlcContext
            //VlcContext.Initialize();
        }

        private void Stop(Object dummy)
        {
            this._VLCcontrol.Stop();
            while (this._VLCcontrol.Medias.Count > 0)
            {
                this._VLCcontrol.Medias.RemoveAt(0);
            }
        }

        private void ExecuteHandled(Action method)
        {
            try
            {
                method.Invoke();
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
            }
        }

        private void Player_OnMouseMove(object sender, MouseEventArgs e)
        {
            this._viewModel.MouseMove();
        }

        #region Buttons click

        private void UiPlayButton_OnClick(object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Media = new PathMedia(this._viewModel.CurrentVideo.FileName);
            this._viewModel.IsPaused = false;
        }

        private void UiPauseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Pause();
            this._viewModel.IsPaused = true;
        }

        private void UiMuteButton_OnClick(object sender, RoutedEventArgs e)
        {
            this._viewModel.IsMute = !this._viewModel.IsMute;
            this._VLCcontrol.AudioProperties.IsMute = this._viewModel.IsMute;
        }

        private void UiSnapshotButton_OnClick(object sender, RoutedEventArgs e)
        {
            String imgPath = this._viewModel.TemporaryImagePath;
            Boolean isPaused = this._VLCcontrol.IsPaused;
            try
            {
                this._VLCcontrol.Pause();
                Thread.Sleep(100);
                this._VLCcontrol.TakeSnapshot(imgPath,
                    uint.Parse(this._VLCcontrol.VideoProperties.Size.Width.ToString("0")),
                    uint.Parse(this._VLCcontrol.VideoProperties.Size.Height.ToString("0")));
            }
            catch (Exception exc)
            {
                this.Logger().Error(exc.Message);
                this.Logger().Error(exc.Source);
            }
            if (!isPaused)
            {
                this._VLCcontrol.Play();
            }
        }

        #endregion

        #region VLC control events

        private void _VLCcontrol_OnPositionChanged(VlcControl sender, VlcEventArgs<float> e)
        {
            this._viewModel.Position = this._VLCcontrol.Position;
        }

        private void _VLCcontrol_OnSnapshotTaken(VlcControl sender, VlcEventArgs<string> e)
        {
            try
            {
                String imgPath = this._viewModel.TemporaryImagePath;
                if (File.Exists(imgPath))
                {
                    Image img = new Bitmap(imgPath);
                    this._viewModel.CurrentVideo.PreviewImage = img;
                    img.Dispose();
                    File.Delete(imgPath);
                }
            }
            catch (Exception ex)
            {
                this.Logger().ErrorFormat(ex.Message);
            }
        }

        private void _VLCcontrol_OnLengthChanged(VlcControl sender, VlcEventArgs<long> e)
        {
            this._eventAggregator.GetEvent<VideoDurationChanged>().Publish(this._VLCcontrol.Duration);
        }

        private void _VLCcontrol_OnEndReached(VlcControl sender, VlcEventArgs<EventArgs> e)
        {
            this._eventAggregator.GetEvent<VideoEnded>().Publish(null);
        }

        #endregion

        #region Slider

        private void UiPositionSlider_PreviewMouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            this._viewModel.IsMouseDown = true;
            this._VLCcontrol.PositionChanged -= this._VLCcontrol_OnPositionChanged;
        }

        private void UiPositionSlider_PreviewMouseLeftButtonUp(Object sender, MouseButtonEventArgs e)
        {
            this._viewModel.IsMouseDown = false;
            this._VLCcontrol.PositionChanged += this._VLCcontrol_OnPositionChanged;
        }

        #endregion
    }
}