using VideoPlayer.Database.Repository;
using VideoPlayer.Nfo;
using VideoPlayer.Services;

namespace VideoPlayer.NfoExtractor;
class Program
{
    static void Main(String[] args)
    {
        var filePath = args[0];
        if (File.Exists(filePath))
        {
            var repo = new FileVideoRepository();
            var ofw = repo.Load(filePath);
            var videos = ofw.Videos;
            var nfoSerializer = new NfoSerializer();
            var nfoService = new NfoService(nfoSerializer);
            nfoService.CreateNfo(videos);
        }
        else
        {
            Console.WriteLine($"File '{filePath}' not found.");
        }
        Console.ReadLine();
    }
}
