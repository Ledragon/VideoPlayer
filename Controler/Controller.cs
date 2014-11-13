using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Classes;
using Log;
using VideoPlayer.Bootstrapper;
using VideoPlayer.Common;
using VideoPlayer.Database.Repository;
using VideoPlayer.Helpers;
using Directory = Classes.Directory;

namespace Controllers
{
    public class Controller
    {
        //private readonly IVideoRepository _repository;

        //private readonly String[] _videoExtensions =
        //{
        //    ".3gp", ".asf", ".avi", ".divx", ".flv", ".m4v", ".mov", ".mp4",
        //    ".mpeg", ".mpg", ".webm", ".wmv"
        //};

        //public Controller()
        //{
        //    this._repository = DependencyFactory.Resolve<IVideoRepository>();
        //}

        //public void Save(ObjectsWrapper wrapper)
        //{
        //    this.Save(FileSystemHelper.GetDefaultFileName(), wrapper);
        //}

        //public void Save(String filePath, ObjectsWrapper wrapper)
        //{
        //    this._repository.Save(filePath, wrapper);
        //}

        //public ObjectsWrapper GetObjectsFromFile()
        //{
        //    var wrapper =  this.GetObjectsFromFile(FileSystemHelper.GetDefaultFileName());
        //    return wrapper;
        //}

        //public ObjectsWrapper GetObjectsFromFile(String filePath)
        //{
        //    return this._repository.Load(filePath);
        //}

        //public List<String> GetVideoFiles(String directory)
        //{
        //    var videoFiles = new List<String>();
        //    try
        //    {
        //        var directoryInfo = new DirectoryInfo(directory);
        //        videoFiles.AddRange(from fileInfo in directoryInfo.GetFiles("*", SearchOption.AllDirectories)
        //            where Array.IndexOf(this._videoExtensions, fileInfo.Extension.ToLowerInvariant()) != -1
        //            select fileInfo.FullName);
        //    }
        //    catch (Exception e)
        //    {
        //        this.Logger().Error(e.Message);
        //        this.Logger().Error(e.Source);
        //    }
        //    return videoFiles;
        //}

        //public List<String> GetVideoFiles(Directory directory)
        //{
        //    var videoFiles = DirectoryHelper.GetVideoFiles(directory.DirectoryPath, directory.IsIncludeSubdirectories);
        //    return videoFiles;
        //}
    }
}