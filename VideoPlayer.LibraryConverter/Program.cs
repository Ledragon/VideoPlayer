using LeDragon.Log.Standard;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Net;
using VideoPlayer.Database.Repository;
using VideoPlayer.Database.Repository.SQLite;
using VideoPlayer.Entities;
using VideoPlayer.LibraryConverter;
using VideoPlayer.Services;

LoggingSystemManager.SetPath("./converter.log");
var logger = LoggerFactory.Logger<Program>();
try
{

    var settings = new JsonFileManager().Deserialize("./settings.json");
    var libraryFile = settings.SourceFile;
    var replacer = new PathReplacer();

    var xmlRepo = new FileLibraryRepository();
    var objects = xmlRepo.Load(libraryFile);
    logger.Info($"Converting file {{{libraryFile}}}");
    if (File.Exists(settings.TargetFile))
    {
        logger.Info($"Deleting file {{{settings.TargetFile}}}");
        File.Delete(settings.TargetFile);
    }
    using (var context = new VideoPlayerContext(settings.TargetFile))
    {
        context.Database.EnsureCreated();
        Dictionary<String, EntityEntry<VideoPlayer.Entities.Directory>> dic = CreateDirectories(settings, objects, context, logger);
        objects.Videos.ForEach((Action<Video>)(v =>
        {
            CreateVideo(v, settings, replacer, dic, logger);
        }));

        var videos = objects.Videos;
        var tags = new List<Tag>();
        var categories = videos
            .GroupBy(v => !String.IsNullOrEmpty(v.Category) ? v.Category : "(empty)")
            .ToDictionary(d => d.Key, d => d.ToList());
        var byTagDic = videos
            .Where(v => v.Tags != null && v.Tags.Any())
            .SelectMany(v => v.Tags.Where(t => !String.IsNullOrEmpty(t.Value)).Select(t => t.Value))
            .Distinct()
            .ToDictionary(t => t, t => videos.Where(v => v.Tags != null && v.Tags.Any(tag => tag.Value == t)).ToList());
        videos.ForEach(v =>
        {
            if (v.Tags?.Any() != null)
            {
                v.Tags.Clear();
            }
        });
        foreach (var category in categories)
        {
            var tag = new Tag
            {
                Value = category.Key,
                Videos = category.Value
            };
            category.Value.ForEach(v => v.Tags.Add(tag));
            tags.Add(tag);
        }
        foreach (var btd in byTagDic)
        {
            var tag = tags.FirstOrDefault(t => t.Value == btd.Key);
            if (tag != null)
            {
                tag.Videos = tag.Videos.Concat(btd.Value).ToList();
            }
            else
            {
                tag = new Tag
                {
                    Value = btd.Key,
                    Videos = btd.Value
                };
                tags.Add(tag);
            }
            btd.Value.ForEach(v => v.Tags.Add(tag));
        }
        if (objects.Videos.Any(v => v.Directory == null))
        {
            logger.Warn($"Some videos could not be mapped to new directory. Aborting");
        }
        else
        {
            context.Tags.AddRange(tags);
            context.Videos.AddRange(objects.Videos);
            context.SaveChanges();
        }
    }
}
catch (Exception e)
{
    logger.Error(e.Message);
    while (e.InnerException != null)
    {
        logger.Error(e.InnerException.Message);
        e = e.InnerException;
    }
    throw;
}

static void CreateVideo(Video v, JsonSettingsFile settings, PathReplacer replacer, Dictionary<String, EntityEntry<VideoPlayer.Entities.Directory>> dic, ILogger logger)
{
    logger.Info($"Adding video {{{v.FileName}}}");
    if ((v.Thumbnails == null))
    {
        v.Thumbnails = new List<Thumbnail>();
    }
    if (v.Thumbnails.All(t => t.Image != v.SerializedImage))
    {
        v.Thumbnails.Add(new Thumbnail { Image = v.SerializedImage });
    }
    var firstMatch = settings.Remap.Keys.FirstOrDefault(d => v.FileName.StartsWith(d));
    if (!String.IsNullOrEmpty(firstMatch))
    {
        replacer.ReplaceDirectory(firstMatch, settings.Remap[firstMatch], v);
    }
    else
    {
        logger.Warn($"No replacement match found for {{{v.FileName}}}");
    }
    if (dic.TryGetValue(v.FileName, out var entity))
    {
        entity.Entity.Videos.Add(v);
        v.Directory = entity.Entity;
    }
    else
    {
        logger.Warn($"No directory found for {{{v.FileName}}}");
    }

    var cs = v.FileName + ".png";
    if (File.Exists(cs))
    {
        var bytes = File.ReadAllBytes(cs);
        var converted = Convert.ToBase64String(bytes);
        v.ContactSheet = new ContactSheet { Image = converted };
    }
}

static Dictionary<String, EntityEntry<VideoPlayer.Entities.Directory>> CreateDirectories(JsonSettingsFile settings, ObjectsWrapper objects, VideoPlayerContext context, ILogger logger)
{
    var dic = new Dictionary<String, EntityEntry<VideoPlayer.Entities.Directory>>();
    foreach (var directory in objects.Directories)
    {
        logger.Info($"Remapping directory {{{directory.DirectoryPath}}}");
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

    return dic;
}