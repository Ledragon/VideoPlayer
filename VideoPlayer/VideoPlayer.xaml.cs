using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HomeModule;
using Log;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Common;
using VideoPlayer.Infrastructure;
using VideoPlayer.Services;

namespace VideoPlayer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        public MainWindow()
        {
            this.InitializeComponent();
            try
            {
                this.DataContext = DependencyFactory.Resolve<IVideoPlayerViewModel>();
                var eventAggregator = DependencyFactory.Resolve<IEventAggregator>();
                eventAggregator.GetEvent<CloseRequestedEvent>().Subscribe((dummy) =>
                {
                    this.Close();
                });
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //if (!(e.OriginalSource is TextBox))
            //{
            //    if (e.Key == Key.S)
            //    {
            //        var mbr = MessageBox.Show("Do you really want to exit?", "Exit?",
            //            MessageBoxButton.YesNo);
            //        if (mbr == MessageBoxResult.Yes)
            //        {
            //            this.Close();
            //        }
            //        e.Handled = true;
            //    }
            //}
        }
    }
}