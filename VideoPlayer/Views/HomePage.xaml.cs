using System.Windows;
using System.Windows.Controls;

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage
    {
        public static RoutedEvent VideoClickEvent = EventManager.RegisterRoutedEvent("VideoClick",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (HomePage));

        public event RoutedEventHandler VideoClick
        {
            add { AddHandler(VideoClickEvent, value); }
            remove { this.RemoveHandler(VideoClickEvent, value); }
        }

        protected virtual void RaiseVideoClickEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(VideoClickEvent);
            this.RaiseEvent(args);
        }

        public HomePage()
        {
            InitializeComponent();
        }

        private void _uiVideosButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseVideoClickEvent();
        }

        private void _uiSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseSettingsClickEvent();
        }


        public static RoutedEvent SettingsClickEvent = EventManager.RegisterRoutedEvent("SettingsClick",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(HomePage));

        public event RoutedEventHandler SettingsClick
        {
            add { AddHandler(SettingsClickEvent, value); }
            remove { this.RemoveHandler(SettingsClickEvent, value); }
        }

        protected virtual void RaiseSettingsClickEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(SettingsClickEvent);
            this.RaiseEvent(args);
        }



        public static RoutedEvent CleanClickEvent = EventManager.RegisterRoutedEvent("CleanClick",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(HomePage));

        public event RoutedEventHandler CleanClick
        {
            add { AddHandler(CleanClickEvent, value); }
            remove { this.RemoveHandler(CleanClickEvent, value); }
        }

        protected virtual void RaiseCleanClickEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(CleanClickEvent);
            this.RaiseEvent(args);
        }



        public static RoutedEvent LoadClickEvent = EventManager.RegisterRoutedEvent("LoadClick",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(HomePage));

        public event RoutedEventHandler LoadClick
        {
            add { AddHandler(LoadClickEvent, value); }
            remove { this.RemoveHandler(LoadClickEvent, value); }
        }

        protected virtual void RaiseLoadClickEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(LoadClickEvent);
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
    }
}
