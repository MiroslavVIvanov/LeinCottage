namespace LeinCottage.Web.Controllers
{
    using Common;
    using CustomAttributes;
    using Data;
    using Models;
    using System.Drawing;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;

    public class ImagesAdministrationController : Controller
    {
        private const string PhotosDirectoryName = "GalleryPhotos";
        private const string ThumbsDirectoryName = "GalleryThumbnails";

        [BasicAuthentication("", "")]
        public ActionResult Index()
        {
            CheckIfDirectoryExist(PhotosDirectoryName);
            CheckIfDirectoryExist(ThumbsDirectoryName);

            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                //names and paths
                string rootPath = Server.MapPath("~");

                string originalName = file.FileName;

                string photoName = PhotoNameProvider.GetName(file.FileName);
                string photoPath = Path.Combine(rootPath, PhotosDirectoryName, photoName);

                string thumbName = PhotoNameProvider.GetThumbnailName(file.FileName);
                string thumbPath = Path.Combine(rootPath, ThumbsDirectoryName, thumbName);

                //image and resizing
                ImageProcessor.SavePhoto(file.InputStream, photoPath);
                ImageProcessor.SavePhotoThumbnail(file.InputStream, thumbPath);

                //photo to database
                var photos = new EfGenericRepository<Photo>(new LeinCottageDbContext());
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

            return RedirectToAction("Index");
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