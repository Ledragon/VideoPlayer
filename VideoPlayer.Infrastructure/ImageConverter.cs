using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Log;

namespace VideoPlayer.Infrastructure
{
    /// <summary>
    ///     One-way converter from System.Drawing.Image to System.Windows.Media.ImageSource
    /// </summary>
    [ValueConversion(typeof (Image), typeof (ImageSource))]
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var bitmap = new BitmapImage();
            // empty images are empty...
            try
            {
                if (value == null)
                {
                    return null;
                }

                var image = (Image)value;
                // Winforms Image we want to get the WPF Image from...
                bitmap.BeginInit();
                var memoryStream = new MemoryStream();
                // Save to a memory stream...
                image.Save(memoryStream, image.RawFormat);
                // Rewind the stream...
                memoryStream.Seek(0, SeekOrigin.Begin);
                bitmap.StreamSource = memoryStream;
                bitmap.EndInit();
            }
            catch (Exception e)
            {
            }
            return bitmap;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}