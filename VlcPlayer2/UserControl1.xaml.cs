using System;
using System.IO;
using System.Windows.Controls;

namespace VlcPlayer2
{
    /// <summary>
    ///     Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            this.InitializeComponent();
            this.InitializeVlc();
        }

        private void InitializeVlc()
        {
            String vlcLibDirectory;
            if (IntPtr.Size == 4)
            {
                // Use 32 bits library
                vlcLibDirectory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "libvlc_x86")).FullName;
            }
            else
            {
                // Use 64 bits library
                vlcLibDirectory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "libvlc_x64")).FullName;
            }

            var options = new String[]
            {
                // VLC options can be given here. Please refer to the VLC command line documentation.
            };

            this.MyControl.SourceProvider.CreatePlayer(vlcLibDirectory, options);

            // Load libvlc libraries and initializes stuff. It is important that the options (if you want to pass any) and lib directory are given before calling this method.
            this.MyControl.SourceProvider.MediaPlayer.Play(
                "http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_h264.mov");
        }
    }
}