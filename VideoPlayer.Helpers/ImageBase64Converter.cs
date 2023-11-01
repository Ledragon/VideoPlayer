using System;

namespace VideoPlayer.Helpers
{
    public static class ImageBase64Converter
    {
        public static String ToBase64(String imagePath)
        {
            if (System.IO.File.Exists(imagePath))
            {
                var bytes = System.IO.File.ReadAllBytes(imagePath);
                var converted = Convert.ToBase64String(bytes);
                return converted;
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
