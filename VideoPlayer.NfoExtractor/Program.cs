namespace VideoPlayer.NfoExtractor;
class Program
{
    static void Main(String[] args)
    {
        if (args.Length == 3)
        {
            var filePath = args[0];
            var sourceDir = args[1];
            var targetDir = args[2];
            if (File.Exists(filePath))
            {
                var service = new Service();
                service.GenerateNfo(filePath, sourceDir, targetDir);
            }
            else
            {
                Console.WriteLine($"File '{filePath}' not found.");
            }
        }
        else
        {
            Console.WriteLine("Expected 3 arguments: video file path, source directory, target directory");
        }
        Console.ReadLine();
    }
}
