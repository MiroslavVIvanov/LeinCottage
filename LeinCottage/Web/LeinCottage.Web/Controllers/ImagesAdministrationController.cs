﻿namespace LeinCottage.Web.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Common;
    using CustomAttributes;
    using Models;
    using LocalData;
    using Exceptions;

    [NoCache]
    public class ImagesAdministrationController : Controller
    {
        private const string PhotosDirectoryName = "GalleryPhotos";
        private const string ThumbsDirectoryName = "GalleryThumbnails";

        [BasicAuthentication("", "")]
        public ActionResult Index()
        {
            this.CheckIfDirectoryExist(PhotosDirectoryName);
            this.CheckIfDirectoryExist(ThumbsDirectoryName);

            var photos = JsonPhotoRepository<Photo>.Instance;
            var allPhotos = photos.All().OrderByDescending(p => p.Id);

            return this.View(allPhotos);
        }

        public ActionResult PhotoUploadError()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<HttpPostedFileBase> uploadedPhotos)
        {
            var a = uploadedPhotos;
            if (uploadedPhotos != null && uploadedPhotos.Count() > 0)
            {
                //var photos = new EfGenericRepository<Photo>(new LeinCottageDbContext());
                var photos = JsonPhotoRepository<Photo>.Instance;
                foreach (var file in uploadedPhotos)
                {
                    if (file.ContentLength > 0)
                    {
                        // names and paths
                        string rootPath = Server.MapPath("~");

                        string originalName = file.FileName;

                        string photoName = PhotoNameProvider.GetName(file.FileName);
                        string photoPath = Path.Combine(rootPath, PhotosDirectoryName, photoName);

                        string thumbName = PhotoNameProvider.GetThumbnailName(file.FileName);
                        string thumbPath = Path.Combine(rootPath, ThumbsDirectoryName, thumbName);

                        // image and resizing
                        try
                        {
                            ImageProcessor.SavePhoto(file.InputStream, photoPath);
                            ImageProcessor.SavePhotoThumbnail(file.InputStream, thumbPath);
                        }
                        catch (FileNotPhotoException ex)
                        {
                            return View("PhotoUploadError");
                        }

                        // photo to database
                        var newPhoto = new Photo()
                        {
                            IsVisible = true,
                            Name = photoName,
                            ThumbnailName = thumbName,
                            OriginalName = originalName
                        };

                        photos.Add(newPhoto);
                        photos.SaveChanges();
                    }
                }
            }

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public void Delete(int id)
        {
            try
            {
                var photos = JsonPhotoRepository<Photo>.Instance;
                var path = Path.Combine(Server.MapPath("~"), "GalleryPhotos", photos.GetById(id).Name);
                var thumbPath = Path.Combine(Server.MapPath("~"), "GalleryThumbnails", photos.GetById(id).ThumbnailName);

                ImageProcessor.PhysicallyDeletePhoto(path);
                ImageProcessor.PhysicallyDeletePhoto(thumbPath);

                photos.Delete(id);
                photos.SaveChanges();
            }
            catch (System.Exception)
            {
            }
        }

        private void CheckIfDirectoryExist(string directoryName)
        {
            string rootPath = Server.MapPath("~");

            string directoryPath = Path.Combine(rootPath, directoryName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}