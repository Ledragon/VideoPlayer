namespace VideoPlayer.ImageExtractor
{
    public interface IImageExtractionService
    {
        void SaveImage(System.String base64Image, System.String outputFilePath);
    }
}