using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoPlayer.Database.Repository;
using VideoPlayer.Database.Repository.SQLite;
using VideoPlayer.Services;

var xmlRepo = new FileLibraryRepository();

var pathService = new PathService();
var libraryFile = pathService.GetLibraryFile();
var objects = xmlRepo.Load(libraryFile);
using (var context = new VideoPlayerContext(Path.Combine(Path.GetDirectoryName(libraryFile), "Library.db")))
{
    context.Database.EnsureCreated();

    var dic = new Dictionary<String, EntityEntry<VideoPlayer.Entities.Directory>>();
    foreach (var directory in objects.Directories)
    {
        var entity = context.Directories.Add(directory);
        if (Directory.Exists(directory.DirectoryPath))
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
            v.Thumbnails = new List<VideoPlayer.Entities.Thumbnail>();
        }
        if (v.Thumbnails.All(t => t.Image != v.SerializedImage))
        {
            v.Thumbnails.Add(new VideoPlayer.Entities.Thumbnail { Image = v.SerializedImage });
        }
        if (v.Tags == null)
        {
            v.Tags = new List<VideoPlayer.Entities.Tag> { };
        }
        if (!String.IsNullOrEmpty(v.Category))
        {
            var tag = context.Tags.FirstOrDefault(t => t.Value == v.Category)
            ?? context.Tags.Add(new VideoPlayer.Entities.Tag { Value = v.Category }).Entity;
            v.Tags.Add(tag);
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