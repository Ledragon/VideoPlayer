using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Classes;
using Log;
using VideoPlayer.ViewModels;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Medias;
using Vlc.DotNet.Wpf;
using Cursors = System.Windows.Input.Cursors;
using Image = System.Drawing.Image;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using TextBox = System.Windows.Controls.TextBox;
using Timer = System.Windows.Forms.Timer;

namespace VideoPlayer
{
    /// <summary>
    ///     Interaction logic for VideosPage.xaml
    /// </summary>
    public partial class VideosPage
    {
        private ObservableCollection<Video> _videoCollection;

        public ObservableCollection<Video> VideoCollection
        {
            get { return this._videoCollection; }
            set
            {
                this._videoCollection = value;
                this._uiFilesListBox.DataContext = this._videoCollection;
            }
        }

        #region Private Members

        private readonly ObservableCollection<Video> _currentPlayList = new ObservableCollection<Video>();

        private readonly Timer _timer = new Timer();
        private Boolean _isFullScreenVideo;

        private Boolean _isPlaylist;

        private Boolean _isPositionChanging;

        private DateTime _mouseLastMouveDateTime = DateTime.Now;

        private Video NowPlaying
        {
            get
            {
                //ObservableCollection<Video> videos = this.DataContext as ObservableCollection<Video>;
                //String path = new Uri(this._VLCcontrol.Media.MRL).LocalPath;
                //return videos.FirstOrDefault(v => v.FileName == path);
                ObservableCollection<Video> videos = this.VideoCollection;
                String path = new Uri(this._VLCcontrol.Media.MRL).LocalPath;
                return videos.FirstOrDefault(v => v.FileName == path);
            }
        }

        #endregion

        #region Constructors

        public VideosPage()
        {
            // Set libvlc.dll and libvlccore.dll directory path
            String programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            VlcContext.LibVlcDllsPath = Path.Combine(programFilesPath, @"VideoLan\VLC");

            // Set the vlc plugins directory path
            VlcContext.LibVlcPluginsPath = Path.Combine(VlcContext.LibVlcDllsPath, "plugins");

            //// refer to http://wiki.videolan.org/VLC_command-line_help for more information
            VlcContext.StartupOptions.IgnoreConfig = true;
            VlcContext.StartupOptions.AddOption("--no-video-title-show");
            VlcContext.StartupOptions.ScreenSaverEnabled = false;
            VlcContext.StartupOptions.AddOption("--no-snapshot-preview");
            VlcContext.StartupOptions.AddOption("--no-mouse-events");
            VlcContext.StartupOptions.AddOption("--no-keyboard-events");
            VlcContext.StartupOptions.AddOption("--disable-screensaver");
            ////VlcContext.StartupOptions.AddOption("--no-skip-frames");
            //VlcContext.StartupOptions.AddOption("--directx-hw-yuv");
            ////VlcContext.StartupOptions.AddOption("--directx-3buffering");
            //VlcContext.StartupOptions.AddOption("--ffmpeg-skiploopfilter=4");
            //VlcContext.StartupOptions.AddOption("--ffmpeg-hw");
            //VlcContext.StartupOptions.AddOption("--vout=directx");


            ////VlcContext.StartupOptions.ShowLoggerConsole = false;
            ////VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.Debug;
            ////VlcContext.StartupOptions.LogOptions.LogInFile = false;
            ////VlcContext.StartupOptions.LogOptions.LogInFilePath = @"D:\videoplayer_vlc.log";
            //// Initialize the VlcContext
            VlcContext.Initialize();

            this.InitializeComponent();
        }

        #endregion

        #region Events

        private void ListBoxItem_MouseDoubleClick(Object sender, MouseButtonEventArgs e)
        {
            this.PlaySelectedVideo();
        }

        private void UserControl_KeyDown(Object sender, KeyEventArgs e)
        {
            if (!(e.OriginalSource is TextBox))
            {
                if (e.Key == Key.Tab)
                {
                    if (this._isFullScreenVideo)
                    {
                        this.SwitchToWindowMode();
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
                else if (e.Key == Key.A)
                {
                    this.AddToExistingPlaylist();
                    e.Handled = true;
                }
                else if (e.Key == Key.P)
                {
                    this._isPlaylist = true;
                    this.PlaySelectedVideo(false);
                    //this.playlistPosition = 0;
                    e.Handled = true;
                }
                else if (e.Key == Key.C)
                {
                    if (this._uiPlaylist.IsVisible)
                    {
                        this._uiPlaylist.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        this._uiPlaylist.Visibility = Visibility.Visible;
                    }
                    e.Handled = true;
                }
                else if (e.Key == Key.Space)
                {
                    if (this._VLCcontrol.IsPaused)
                    {
                        this._VLCcontrol.Play();
                    }
                    else
                    {
                        this._VLCcontrol.Pause();
                    }
                    e.Handled = true;
                }
                else if (e.Key == Key.E)
                {
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        ICollectionView listCollectionView =
                            CollectionViewSource.GetDefaultView(this._uiFilesListBox.ItemsSource) as ListCollectionView;
                        if (listCollectionView != null)
                        {
                            listCollectionView.Refresh();
                        }
                        ;
                        this._uiVideoInfoTabControl.SelectedIndex = 0;
                        this._uiFilesListBox.Items.Refresh();
                        //this._uiFilesListBox.Items.SortDescriptions.Clear();
                        //this._uiFilesListBox.Items.SortDescriptions.Add(
                        //    new SortDescription(this._uiSortComboBox.SelectedItem.ToString(),
                        //        ListSortDirection.Ascending));
                        //this._uiFilesListBox.Items.SortDescriptions.Add(
                        //    new SortDescription("Title",
                        //        ListSortDirection.Ascending));
                    }
                    else
                    {
                        this._uiVideoInfoTabControl.SelectedIndex = 1;
                    }
                    e.Handled = true;
                }
                else if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
                {
                    if (this._uiFilterGrid.Visibility == Visibility.Collapsed)
                    {
                        ObservableCollection<Video> videos = this.VideoCollection;
                        if (videos != null)
                        {
                            IOrderedEnumerable<string> categories =
                                videos.Select(v => v.Category).Distinct().OrderBy(v => v);

                            this._uiCategoryFilterComboBox.ItemsSource = categories;
                        }
                        this._uiFilterGrid.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this._uiFilterGrid.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void AddToExistingPlaylist()
        {
            this.AddToExistingPlaylist(this._uiFilesListBox.SelectedItem as Video);
        }

        private void AddToExistingPlaylist(Video video)
        {
            this._VLCcontrol.Medias.Add(new PathMedia(video.FileName));
            this._currentPlayList.Add(video);
        }

        private void _VLCcontrol_PositionChanged(VlcControl sender, VlcEventArgs<float> e)
        {
            if (!this._isPositionChanging)
            {
                this._uiFullScreenSlider.Value = this._VLCcontrol.Position;
            }
        }

        private void _VLCcontrol_EndReached(VlcControl sender, VlcEventArgs<EventArgs> e)
        {
            //this.NowPlaying.NumberOfViews++;
            if (!this._isPlaylist)
            {
                this.StopVideoPlaying();
                this._VLCcontrol.EndReached -= this._VLCcontrol_EndReached;
            }
            else
            {
                this._VLCcontrol.Next();
                //this.NowPlaying = this._currentPlayList.SingleOrDefault(v => v.FileName == this._VLCcontrol.Media.MRL);
                this.UpdateInfos();
            }

            // TODO gerer la fin d'une video selon qu'on est en playlist ou pas
        }

        private void _VLCcontrol_SnapshotTaken(VlcControl sender, VlcEventArgs<string> e)
        {
            try
            {
                String imgPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), this.NowPlaying.Title + ".jpg");
                if (File.Exists(imgPath))
                {
                    Image img = new Bitmap(imgPath);
                    this.NowPlaying.PreviewImage = img;
                    img.Dispose();
                    File.Delete(imgPath);
                }
                this._VLCcontrol.SnapshotTaken -= this._VLCcontrol_SnapshotTaken;
            }
            catch (Exception ex)
            {
                this.Logger().ErrorFormat(ex.Message);
            }
        }

        private void UserControl_Loaded(Object sender, RoutedEventArgs e)
        {
            this._uiFilesListBox.Items.SortDescriptions.Add(
                new SortDescription(this._uiSortComboBox.SelectedItem.ToString(), ListSortDirection.Ascending));
            this._uiFilesListBox.Items.SortDescriptions.Add(
                new SortDescription("Title",
                    ListSortDirection.Ascending));
            this._timer.Interval = 1000;
            this._timer.Tick += this.timer_Tick;
            this._VLCcontrol.PositionChanged += this._VLCcontrol_PositionChanged;
            this._uiPlaylist.DataContext = this._currentPlayList;
            this.CreateCategoryListBox();
        }

        private void CreateCategoryListBox()
        {
            var viewModel = new ObservableCollection<CategoryListViewModel>();
            viewModel.Add(new CategoryListViewModel
            {
                Count = this.VideoCollection.Count,
                Name = "All"
            });
            var categories =
                this.VideoCollection.Select(v => new {Name = v.Category}).Distinct().OrderBy(c => c.Name);
            foreach (var category in categories)
            {
                viewModel.Add(new CategoryListViewModel
                {
                    Name = category.Name,
                    Count = this.VideoCollection.Count(v => v.Category == category.Name)
                });
            }
            this.UiCategoriesListBox.ItemsSource = viewModel;
        }

        private void Media_DurationChanged(MediaBase sender, VlcEventArgs<long> e)
        {
            this.NowPlaying.Length = this._VLCcontrol.Duration;
            this._uiDuration.Text = this.NowPlaying.Length.ToString("hh\\:mm\\:ss");
        }

        private void timer_Tick(Object sender, EventArgs e)
        {
            if (DateTime.Now - this._mouseLastMouveDateTime > new TimeSpan(0, 0, 2))
            {
                this.Cursor = Cursors.None;
                this._uiFullScreenControlsGrid.Visibility = Visibility.Hidden;
            }
        }

        #region Buttons

        private void _uiMuteButton_Click(Object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.AudioProperties.IsMute = !this._VLCcontrol.AudioProperties.IsMute;
            if (this._VLCcontrol.AudioProperties.IsMute)
            {
                this._uiFullScreenMuteButton.Content = this.FindResource("Muted");
            }
            else
            {
                this._uiFullScreenMuteButton.Content = this.FindResource("Unmuted");
            }
        }

        private void _uiStopButton_Click(Object sender, RoutedEventArgs e)
        {
            this.StopVideoPlaying();
        }

        private void _uiPauseButton_Click(Object sender, RoutedEventArgs e)
        {
            if (this._VLCcontrol.IsPaused)
            {
                this._VLCcontrol.Play();
            }
            else
            {
                this._VLCcontrol.Pause();
            }
        }

        private void _uiPlayButton_Click(Object sender, RoutedEventArgs e)
        {
            if (this._VLCcontrol.IsPaused)
            {
                this._VLCcontrol.Play();
            }
            else
            {
                this.PlaySelectedVideo();
            }
        }

        private void _uiFasterButton_Click(Object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Rate *= 2;
            this._uiRate.Text = "Rate: " + this._VLCcontrol.Rate;
        }

        private void _uiSlowerButton_Click(Object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Rate /= 2;
            this._uiRate.Text = "Rate: " + this._VLCcontrol.Rate;
        }

        private void _uiNextButton_Click(Object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Next();
            this.UpdateInfos();
        }

        private void _uiPreviousButton_Click(Object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Previous();
            this.UpdateInfos();
        }

        private void _uiSnapshotButton_Click(Object sender, RoutedEventArgs e)
        {
            String imgPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), this.NowPlaying.Title + ".jpg");
            Boolean isPaused = this._VLCcontrol.IsPaused;
            try
            {
                this._VLCcontrol.Pause();
                Thread.Sleep(100);
                this._VLCcontrol.TakeSnapshot(imgPath,
                    uint.Parse(this._VLCcontrol.VideoProperties.Size.Width.ToString("0")),
                    uint.Parse(this._VLCcontrol.VideoProperties.Size.Height.ToString("0")));
                this._VLCcontrol.SnapshotTaken += this._VLCcontrol_SnapshotTaken;
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

        #region Slider

        private void _uiVLCFullScrenGrid_MouseMove(Object sender, MouseEventArgs e)
        {
            this._mouseLastMouveDateTime = DateTime.Now;
            this.Cursor = Cursors.Arrow;
            this._uiFullScreenControlsGrid.Visibility = Visibility.Visible;
        }

        private void _uiSlider_PreviewMouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            this._VLCcontrol.PositionChanged -= this._VLCcontrol_PositionChanged;
            this._isPositionChanging = true;
        }

        private void _uiSlider_PreviewMouseLeftButtonUp(Object sender, MouseButtonEventArgs e)
        {
            this._VLCcontrol.Position = (float) this._uiFullScreenSlider.Value;
            this._VLCcontrol.PositionChanged += this._VLCcontrol_PositionChanged;
            this._isPositionChanging = false;
        }

        private void _uiSlider_ValueChanged(Object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this._isPositionChanging)
            {
                this._VLCcontrol.Position = (float) e.NewValue;
            }
            Int32 seconds =
                Int32.Parse((this._uiFullScreenSlider.Value*this._VLCcontrol.Duration.TotalSeconds).ToString("0"));
            var position = new TimeSpan(0, 0, seconds);
            this._uiPosition.Text = position.ToString("hh\\:mm\\:ss");
        }

        private void _uiSlider_PreviewMouseWheel(Object sender, MouseWheelEventArgs e)
        {
            this._VLCcontrol.PositionChanged -= this._VLCcontrol_PositionChanged;
            this._isPositionChanging = true;

            if (e.Delta < 0)
            {
                this._uiFullScreenSlider.Value -= this._uiFullScreenSlider.SmallChange;
            }
            else
            {
                this._uiFullScreenSlider.Value += this._uiFullScreenSlider.SmallChange;
            }
            this._VLCcontrol.PositionChanged += this._VLCcontrol_PositionChanged;
            this._isPositionChanging = false;
        }

        #endregion

        #endregion

        #region Methods

        private void StopVideoPlaying()
        {
            this._VLCcontrol.Stop();
            this._uiVLCFullScrenGrid.MouseMove -= this._uiVLCFullScrenGrid_MouseMove;
            this._uiFullScreenSlider.Value = 0;
            this._VLCcontrol.Medias.Clear();
            this._VLCcontrol.Medias.RemoveAt(0);
            this._currentPlayList.Clear();
            this._uiNowPlaying.Text = "Now playing: ";
            this.SwitchToWindowMode();
        }

        private void PlaySelectedVideo(Boolean isNewPlaylist)
        {
            this._uiVLCFullScrenGrid.MouseMove += this._uiVLCFullScrenGrid_MouseMove;
            if (this._VLCcontrol.IsPlaying)
            {
                this._VLCcontrol.Stop();
                this._uiFullScreenSlider.Value = 0;
            }

            var video = this._uiFilesListBox.SelectedItem as Video;
            if (File.Exists(video.FileName))
            {
                if (isNewPlaylist)
                {
                    this._VLCcontrol.Media = new PathMedia(video.FileName);
                    this._currentPlayList.Clear();
                    this._currentPlayList.Add(video);
                    if (this._VLCcontrol.Media.Duration == TimeSpan.Zero)
                    {
                        this._VLCcontrol.Media.DurationChanged += this.Media_DurationChanged;
                    }
                    this._isPlaylist = false;
                }
                this._VLCcontrol.EndReached += this._VLCcontrol_EndReached;
                this._VLCcontrol.Play();
                this.UpdateInfos();
                this.SwitchToFullScreen();
            }
            else
            {
                MessageBox.Show("File not found");
                this.Logger().ErrorFormat("Could not find file {0}", video.FileName);
            }
        }

        private void PlaySelectedVideo()
        {
            this.PlaySelectedVideo(true);
        }

        private void SwitchToFullScreen()
        {
            this._isFullScreenVideo = true;
            this._uiVideosTabControl.SelectedItem = this._uiVideoPlaying;
            this._timer.Start();
        }

        private void SwitchToWindowMode()
        {
            this._isFullScreenVideo = false;
            this._uiVideosTabControl.SelectedItem = this._uiVideosList;
            this._uiFilesListBox.Focus();
            this._uiFilesListBox.SelectedItem = this.NowPlaying;
            this._timer.Stop();
            this.Cursor = Cursors.Arrow;
        }

        private void UpdateInfos()
        {
            //TODO enlever la methode ci-dessous et faire du binding correct
            this._uiDuration.Text = this.NowPlaying.Length.ToString("hh\\:mm\\:ss");
            this._uiNowPlaying.Text = "Now playing: " + this.NowPlaying.Title;
            this._uiPlaylist.SelectedItem = this.NowPlaying;
        }

        #endregion

        private void UserControl_IsVisibleChanged(Object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this._uiFilesListBox.Visibility == Visibility.Visible)
            {
                this._uiFilesListBox.Focus();
                this._uiFilesListBox.SelectedIndex = 0;
            }
        }

        private void _uiFilterButton_Click(Object sender, RoutedEventArgs e)
        {
            ICollectionView view = this._uiFilesListBox.Items;
            view.Filter = this.Filter;
        }

        private Boolean Filter(Object item)
        {
            Boolean result = true;
            try
            {
                var video = item as Video;
                Boolean isNameOk = true;
                Boolean isTagOk = true;
                if (this._uiFilterGrid.Visibility == Visibility.Visible)
                {
                    isNameOk = video.Title.ToLower().Contains(this._uiFilterTextBox.Text);
                    isTagOk = this.IsTagOk(video);
                }
                Boolean isCategoryOk = this.IsCategoryOk(video);
                result = isNameOk && isTagOk && isCategoryOk;
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
            }
            return result;
        }

        private Boolean IsCategoryOk(Video video)
        {
            string categoryFilter = this._uiCategoryFilterComboBox.SelectedItem.ToString().ToLower();
            Boolean isCategoryOk = true;
            if (!String.IsNullOrEmpty(categoryFilter) && !String.IsNullOrEmpty(video.Category))
            {
                isCategoryOk = video.Category.ToLower() == categoryFilter;
            }
            return isCategoryOk;
        }

        private Boolean IsCategoryOk(Video video, String category)
        {
            Boolean isCategoryOk = true;
            if (!String.IsNullOrEmpty(category) && !String.IsNullOrEmpty(video.Category))
            {
                isCategoryOk = video.Category == category;
            }
            return isCategoryOk;
        }

        private Boolean IsTagOk(Video video)
        {
            Boolean isTagOk = false;
            if (!String.IsNullOrEmpty(this._uiTagFilterTextBox.Text))
            {
                string tagFilter = this._uiTagFilterTextBox.Text.ToLower();
                if (!String.IsNullOrEmpty(tagFilter))
                {
                    string[] filters = tagFilter.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string filter in filters)
                    {
                        isTagOk = isTagOk || video.Tags.Any(t => t.Value.ToLower().Contains(filter));
                    }
                }
                else
                {
                    isTagOk = true;
                }
            }
            else
            {
                isTagOk = true;
            }
            return isTagOk;
        }

        private void _uiPlayAll_OnClick(Object sender, RoutedEventArgs e)
        {
            foreach (Object item in this._uiFilesListBox.Items)
            {
                this.AddToExistingPlaylist(item as Video);
            }
            this.PlaySelectedVideo(false);
        }

        private void _uiVideoPlaying_KeyUp(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X)
            {
                this.StopVideoPlaying();
                e.Handled = true;
            }
        }

        private void _uiSortComboBox_OnSelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (this._uiFilesListBox != null)
            {
                this._uiFilesListBox.Items.SortDescriptions.Clear();
                SortDescription sortDescription = this.SortDescription();
                this._uiFilesListBox.Items.SortDescriptions.Add(sortDescription);
            }
        }

        private SortDescription SortDescription()
        {
            SortDescription sortDescription;
            if (this._uiSortComboBox.SelectedItem.ToString() == "Newest")
            {
                sortDescription = new SortDescription("DateAdded", ListSortDirection.Descending);
            }
            else if (this._uiSortComboBox.SelectedItem.ToString() == "Oldest")
            {
                sortDescription = new SortDescription("DateAdded", ListSortDirection.Ascending);
            }
            else
            {
                sortDescription = new SortDescription(this._uiSortComboBox.SelectedItem.ToString(),
                    ListSortDirection.Ascending);
            }
            return sortDescription;
        }

        private void _uiClearFilter_OnClick(Object sender, RoutedEventArgs e)
        {
            ICollectionView view = this._uiFilesListBox.Items;
            view.Filter = this.TrueFilter;
        }

        private Boolean TrueFilter(Object item)
        {
            return true;
        }

        private void CategoriesListboxItem_MouseLeftButtonUp(Object sender, MouseButtonEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(this._uiFilesListBox.ItemsSource);
            var categoryListViewModel = this.UiCategoriesListBox.SelectedItem as CategoryListViewModel;
            this.Filter(view, categoryListViewModel);
        }

        private void Filter(ICollectionView view, CategoryListViewModel categoryListViewModel)
        {
            if (view != null)
            {
                if (categoryListViewModel.Name == "All")
                {
                    view.Filter = this.TrueFilter;
                }
                else
                {
                    view.Filter = this.FilterCategory;
                }
            }
        }

        private Boolean FilterCategory(Object item)
        {
            Boolean result = true;
            try
            {
                var video = item as Video;
                var categoryListViewModel = this.UiCategoriesListBox.SelectedItem as CategoryListViewModel;
                if (video != null && categoryListViewModel != null)
                {
                    String category = categoryListViewModel.Name;
                    Boolean isCategoryOk = this.IsCategoryOk(video, category);
                    result = isCategoryOk;
                }
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
            }
            return result;
        }

        private void UiPlayAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (Object item in this._uiFilesListBox.Items)
            {
                this.AddToExistingPlaylist(item as Video);
            }
            this.PlaySelectedVideo(false);
        }

    }
}