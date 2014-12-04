//using Vlc.DotNet.Core;
//using Vlc.DotNet.Core.Medias;
//using Vlc.DotNet.Wpf;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Classes;
using Log;
using VideoPlayer.ViewModels;

namespace VideoPlayer
{
    /// <summary>
    ///     Interaction logic for VideosPage.xaml
    /// </summary>
    public partial class VideosPage
    {
        private CategoryListViewModel _categoryListViewModel;
        private ObservableCollection<Video> _videoCollection;
        private readonly VideosTabControlViewModel _videosTabControlViewModel;

        private Video CurrentVideo
        {
            get { return this._uiFilesListBox.SelectedItem as Video; }
        }

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

        //private readonly ObservableCollection<Video> _currentPlayList = new ObservableCollection<Video>();

        //private readonly Timer _timer = new Timer();
        private Boolean _isFullScreenVideo;

        //private Boolean _isPlaylist;

        private Boolean _isPositionChanging;
        private Video _currentVideo;

        //private DateTime _mouseLastMouveDateTime = DateTime.Now;

        //private Video NowPlaying
        //{
        //    get
        //    {
        //        //ObservableCollection<Video> videos = this.DataContext as ObservableCollection<Video>;
        //        //String path = new Uri(this._VLCcontrol.Media.MRL).LocalPath;
        //        //return videos.FirstOrDefault(v => v.FileName == path);
        //        ObservableCollection<Video> videos = this.VideoCollection;
        //        String path = new Uri(this._VLCcontrol.Media.MRL).LocalPath;
        //        return videos.FirstOrDefault(v => v.FileName == path);
        //    }
        //}

        #endregion

        #region Constructors

        public VideosPage()
        {
            this.InitializeComponent();
            this.Player.Stopped += this.Player_Stopped;
            this._videosTabControlViewModel = new VideosTabControlViewModel();
            this.DataContext = this._videosTabControlViewModel;
        }


        #endregion

        private void Player_Stopped(object sender, EventArgs e)
        {
            this.StopVideoPlaying();
        }

        #region Events

        private void ListBoxItem_MouseDoubleClick(Object sender, MouseButtonEventArgs e)
        {
            var video = this.CurrentVideo;
            if (video != null && File.Exists(video.FileName))
            {
                this.Player.PlayVideo(video);
                this.SwitchToFullScreen();
            }
            else
            {
                MessageBox.Show("File not found");
                //this.Logger().ErrorFormat("Could not find file {0}", video.FileName);
            }
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
                    this.SwitchToFullScreen();
                    this.Player.PlayVideo(this.CurrentVideo);
                    e.Handled = true;
                }
                else if (e.Key == Key.A)
                {
                    this.AddToExistingPlaylist();
                    e.Handled = true;
                }
                else if (e.Key == Key.P)
                {
                    this.SwitchToFullScreen();
                    this.Player.PlayAll();
                    //this._isPlaylist = true;
                    //this.PlaySelectedVideo(false);
                    //this.playlistPosition = 0;
                    e.Handled = true;
                }
                //else if (e.Key == Key.C)
                //{
                //    if (this._uiPlaylist.IsVisible)
                //    {
                //        this._uiPlaylist.Visibility = Visibility.Collapsed;
                //    }
                //    else
                //    {
                //        this._uiPlaylist.Visibility = Visibility.Visible;
                //    }
                //    e.Handled = true;
                //}
                    //else if (e.Key == Key.Space)
                    //{
                    //    if (this._VLCcontrol.IsPaused)
                    //    {
                    //        this._VLCcontrol.Play();
                    //    }
                    //    else
                    //    {
                    //        this._VLCcontrol.Pause();
                    //    }
                    //    e.Handled = true;
                    //}
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
            this.Player.AddVideo(this.CurrentVideo);
        }

        private void UserControl_Loaded(Object sender, RoutedEventArgs e)
        {
            this._uiFilesListBox.Items.SortDescriptions.Add(
                new SortDescription(this._uiSortComboBox.SelectedItem.ToString(), ListSortDirection.Ascending));
            this._uiFilesListBox.Items.SortDescriptions.Add(
                new SortDescription("Title",
                    ListSortDirection.Ascending));
        }

        #endregion

        #region Methods

        private void StopVideoPlaying()
        {
            this._uiNowPlaying.Text = "Now playing: ";
            this.SwitchToWindowMode();
        }

        private void SwitchToFullScreen()
        {
            this._videosTabControlViewModel.SelectedIndex = 1;
            this._isFullScreenVideo = true;
            this._uiVideosTabControl.SelectedItem = this._uiVideoPlaying;
        }

        private void SwitchToWindowMode()
        {
            this._videosTabControlViewModel.SelectedIndex = 0;
            this._isFullScreenVideo = false;
            this.Cursor = Cursors.Arrow;
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
            this.PlayAll();
            //foreach (Object item in this._uiFilesListBox.Items)
            //{
            //    this.Player.AddVideo((Video)item);
            //    //this.AddToExistingPlaylist(item as Video);
            //}
            //this.Player.PlayAll();
            //this.PlaySelectedVideo(false);
        }

        private void PlayAll()
        {
            foreach (Object item in this._uiFilesListBox.Items)
            {
                this.Player.AddVideo((Video)item);
            }
            this.Player.PlayAll();
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

        private void Filter(ICollectionView view, CategoryViewModel categoryViewModel)
        {
            if (view != null)
            {
                if (categoryViewModel == null || categoryViewModel.Name == "All")
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
                var categoryListViewModel = this.UiCategoriesListBox.SelectedItem as CategoryViewModel;
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
            this.PlayAll();
            //foreach (Object item in this._uiFilesListBox.Items)
            //{
            //    this.AddToExistingPlaylist(item as Video);
            //}
            //this.PlaySelectedVideo(false);
        }

        private void UiCategoriesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(this._uiFilesListBox.ItemsSource);
            var categoryListViewModel = this.UiCategoriesListBox.SelectedItem as CategoryViewModel;
            this.Filter(view, categoryListViewModel);
        }

        private void UiCategoriesListBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (this._categoryListViewModel == null)
            {
                this._categoryListViewModel = new CategoryListViewModel();
            }
            this.UiCategoriesListBox.DataContext = this._categoryListViewModel;
        }
    }
}