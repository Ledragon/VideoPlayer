namespace VideoPlayer.NfoExtractor
{
    internal class CacheDirectoryProvider
    {
        public static String GetCacheDirectory(String targetDir)
        {
            var cacheDir = Path.Combine(targetDir, "cache");
            if (!Directory.Exists(cacheDir))
            {
                Directory.CreateDirectory(cacheDir);
            }
            return cacheDir;
        }
    }
}
