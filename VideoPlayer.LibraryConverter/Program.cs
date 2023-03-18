using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoPlayer.Database.Repository;
using VideoPlayer.Database.Repository.SQLite;
using VideoPlayer.Entities;
using VideoPlayer.LibraryConverter;
using VideoPlayer.Services;

var settings = new JsonFileManager().Deserialize("./settings.json");
var libraryFile = settings.SourceFile;
var replacer = new PathReplacer();

var xmlRepo = new FileLibraryRepository();
var objects = xmlRepo.Load(libraryFile);
using (var context = new VideoPlayerContext(settings.TargetFile))
{
    context.Database.EnsureCreated();

    var dic = new Dictionary<String, EntityEntry<VideoPlayer.Entities.Directory>>();
    foreach (var directory in objects.Directories)
    {
        var originalPath = directory.DirectoryPath;
        if (settings.Remap.ContainsKey(originalPath))
        {
            directory.DirectoryPath = settings.Remap[originalPath];
        }
        var entity = context.Directories.Add(directory);
        if (System.IO.Directory.Exists(directory.DirectoryPath))
        {
            var di = new DirectoryInfo(directory.DirectoryPath);
            var fileInfos = di.GetFiles("*", SearchOption.AllDirectories);
            foreach (var fileInfo in fileInfos.Where(f => !dic.ContainsKey(f.FullName)))
            {
                dic.Add(fileInfo.FullName, entity);
            }
        }
    }

    objects.Videos.ForEach(v =>
    {
        if ((v.Thumbnails == null))
        {
            v.Thumbnails = new List<Thumbnail>();
        }
        if (v.Thumbnails.All(t => t.Image != v.SerializedImage))
        {
            v.Thumbnails.Add(new Thumbnail { Image = v.SerializedImage });
        }
        if (v.Tags == null)
        {
            v.Tags = new List<Tag> { };
        }
        var category = !String.IsNullOrEmpty(v.Category) ? v.Category : "(empty)";
        var tag = context.Tags.FirstOrDefault(t => t.Value == category) ?? context.Tags.Add(new Tag { Value = category }).Entity;
        v.Tags.Add(tag);
        var firstMatch = settings.Remap.Keys.FirstOrDefault(d => v.FileName.StartsWith(d));
        if (!String.IsNullOrEmpty(firstMatch))
        {
            replacer.ReplaceDirectory(firstMatch, settings.Remap[firstMatch], v);
        }
        if (dic.TryGetValue(v.FileName, out var entity))
        {
            entity.Entity.Videos.Add(v);
            v.Directory = entity.Entity;
        }
    });
    context.Videos.AddRange(objects.Videos);
    context.SaveChanges();
}