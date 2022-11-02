using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class WindowsVideoInfoExtractor
    {
        public VideoViewModel SetShellInfo(VideoViewModel video)
        {
            using (var shellFile = ShellFile.FromFilePath(video.FileName))
            {
                var thumbnail = shellFile.Thumbnail.ExtraLargeBitmap;
                video.PreviewImage = thumbnail;
                var duration = shellFile.Properties.System.Media.Duration.Value;

                Double nanoSeconds = 0;
                if (Double.TryParse(duration.ToString(), out nanoSeconds))
                {
                    var milliSeconds = nanoSeconds * 0.0001;
                    video.Length = TimeSpan.FromMilliseconds(milliSeconds);
                }
                var rating = shellFile.Properties.System.Rating.Value;
                UInt32 myRating = 0;
                if (UInt32.TryParse(rating.ToString(), out myRating))
                {
                    video.Rating = myRating;
                }
            }
            return video;
        }
    }
}
