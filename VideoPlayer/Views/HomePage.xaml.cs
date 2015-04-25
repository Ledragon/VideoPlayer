using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VideoPlayer.Common;
using VideoPlayer.Infrastructure;
using VideoPlayer.ViewModels;

namespace VideoPlayer
{
    /// <summary>
    ///     Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage:IHomePage
    {
        //public static RoutedEvent VideoClickEvent = EventManager.RegisterRoutedEvent("VideoClick",
        //    RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (HomePage));

        //public static RoutedEvent SettingsClickEvent = EventManager.RegisterRoutedEvent("SettingsClick",
        //    RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (HomePage));

        public static RoutedEvent CleanClickEvent = EventManager.RegisterRoutedEvent("CleanClick",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (HomePage));

        public static RoutedEvent LoadClickEvent = EventManager.RegisterRoutedEvent("LoadClick",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (HomePage));

        public HomePage()
        {
            this.InitializeComponent();
            this.DataContext = DependencyFactory.Resolve<IHomePageViewModel>();
        }

        //public event RoutedEventHandler VideoClick
        //{
        //    add { this.AddHandler(VideoClickEvent, value); }
        //    remove { this.RemoveHandler(VideoClickEvent, value); }
        //}

        //protected virtual void RaiseVideoClickEvent()
        //{
        //    var args = new RoutedEventArgs(VideoClickEvent);
        //    this.RaiseEvent(args);
        //}

        //private void _uiVideosButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //this.RaiseVideoClickEvent();
        //}

        //private void _uiSettingsButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //this.RaiseSettingsClickEvent();
        //}


        //public event RoutedEventHandler SettingsClick
        //{
        //    add { this.AddHandler(SettingsClickEvent, value); }
        //    remove { this.RemoveHandler(SettingsClickEvent, value); }
        //}

        //protected virtual void RaiseSettingsClickEvent()
        //{
        //    var args = new RoutedEventArgs(SettingsClickEvent);
        //    this.RaiseEvent(args);
        //}


        public event RoutedEventHandler CleanClick
        {
            add { this.AddHandler(CleanClickEvent, value); }
            remove { this.RemoveHandler(CleanClickEvent, value); }
        }

        protected virtual void RaiseCleanClickEvent()
        {
            var args = new RoutedEventArgs(CleanClickEvent);
            this.RaiseEvent(args);
        }


        public event RoutedEventHandler LoadClick
        {
            add { this.AddHandler(LoadClickEvent, value); }
            remove { this.RemoveHandler(LoadClickEvent, value); }
        }

        protected virtual void RaiseLoadClickEvent()
        {
            var args = new RoutedEventArgs(LoadClickEvent);
            this.RaiseEvent(args);
        }

        private void _uiLoadButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseLoadClickEvent();
        }

        private void _uiCleanButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseCleanClickEvent();
        }

        //private void UiExportButton_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var mainWindow = ((((this.Parent as TabItem).Parent as TabControl).Parent as Grid).Parent as MainWindow);
        //    InfoExporter.ExportImageResolutions(mainWindow.Videos.OrderByDescending(v => v.PreviewImage.Width));
        //}
        public IViewModel ViewModel { get; set; }
    }
}