using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NEwVlcPlayer
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Player : UserControl
    {
        public Player()
        {
            this.InitializeComponent();
            //this.InitializeVlc();
            this._VLCcontrol.MediaPlayer.VlcLibDirectoryNeeded += OnVlcControlNeedsLibDirectory;

        }

        private void OnVlcControlNeedsLibDirectory(Object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {

            String programFilesPath = @"D:\Development\VisualStudio\dll\vlc-2.1.5";
            e.VlcLibDirectory = new DirectoryInfo(programFilesPath);
        }

        private void InitializeVlc()
        {
            //TODO problem with 64bit version of VLC
            //String programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Replace(" (x86)", "");
            //VlcContext.LibVlcDllsPath = Path.Combine(programFilesPath, @"VideoLan\VLC");

            //// Set the vlc plugins directory path
            //VlcContext.LibVlcPluginsPath = Path.Combine(VlcContext.LibVlcDllsPath, "plugins");

            //// refer to http://wiki.videolan.org/VLC_command-line_help for more information
            //VlcContext.StartupOptions.IgnoreConfig = true;
            //VlcContext.StartupOptions.AddOption("--no-video-title-show");
            //VlcContext.StartupOptions.ScreenSaverEnabled = false;
            //VlcContext.StartupOptions.AddOption("--no-snapshot-preview");
            //VlcContext.StartupOptions.AddOption("--no-mouse-events");
            //VlcContext.StartupOptions.AddOption("--no-keyboard-events");
            //VlcContext.StartupOptions.AddOption("--disable-screensaver");
            // Initialize the VlcContext
            //VlcContext.Initialize();
        }

        private void Button_Click(Object sender, RoutedEventArgs e)
        {
            try
            {
                var fileInfo = new FileInfo(@"D:\Guitar videos\[GUITAR] - LESSON - Patrick Rondat - Virtuosité & vélocité à la guitare.avi");
                this._VLCcontrol.MediaPlayer.Play(fileInfo);
            }
            catch (Exception exception)
            {
                throw;
            }
        }
    }
}
