using Newtonsoft.Json;

namespace VideoPlayer.LibraryConverter
{
    internal class JsonFileManager
    {
        public JsonSettingsFile Deserialize(String path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            return JsonConvert.DeserializeObject<JsonSettingsFile>(File.ReadAllText(path));
        }
    }
}
