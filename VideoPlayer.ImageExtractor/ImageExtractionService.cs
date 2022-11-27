using SixLabors.ImageSharp;
using System;
using System.IO;

namespace VideoPlayer.ImageExtractor
{
    public class ImageExtractionService : IImageExtractionService
    {
        public void SaveImage(String base64Image, String outputFilePath)
        {
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
            var data = Convert.FromBase64String(base64Image);
            using (var image = Image.Load(data))
            {
                image.Save(outputFilePath);
            }
        }
    }
}
