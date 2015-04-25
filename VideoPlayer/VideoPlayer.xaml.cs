using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HomeModule;
using Log;
using VideoPlayer.Common;
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
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.OriginalSource is TextBox))
            {
                if (e.Key == Key.S)
                {
                    var mbr = MessageBox.Show("Do you really want to exit?", "Exit?",
                        MessageBoxButton.YesNo);
                    if (mbr == MessageBoxResult.Yes)
                    {
                        this.Close();
                    }
                    e.Handled = true;
                }
            }
        }
    }
}