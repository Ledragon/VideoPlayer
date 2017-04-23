using System;
using HomeModule;
using Log;
using Microsoft.Practices.Prism.PubSubEvents;
using VideoPlayer.Common;
using VideoPlayer.Infrastructure;

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
    }
}