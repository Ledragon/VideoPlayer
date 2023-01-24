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
    });
    context.Videos.AddRange(objects.Videos);
    context.SaveChanges();
}