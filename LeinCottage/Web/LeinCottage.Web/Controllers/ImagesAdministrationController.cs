namespace LeinCottage.Web.Controllers
{
    using Common;
    using CustomAttributes;
    using Data;
    using Models;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    [NoCache]
    public class ImagesAdministrationController : Controller
    {
        private const string PhotosDirectoryName = "GalleryPhotos";
        private const string ThumbsDirectoryName = "GalleryThumbnails";

        [BasicAuthentication("", "")]
        public ActionResult Index()
        {
            CheckIfDirectoryExist(PhotosDirectoryName);
            CheckIfDirectoryExist(ThumbsDirectoryName);

            var photos = new EfGenericRepository<Photo>(new LeinCottageDbContext());
            var allPhotos = photos.All();


            return View(allPhotos);
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

        [HttpPost]
        public void Delete(int id)
        {
            var photos = new EfGenericRepository<Photo>(new LeinCottageDbContext());
            var path = Path.Combine(Server.MapPath("~"), "GalleryPhotos", photos.GetById(id).Name);
            var thumbPath = Path.Combine(Server.MapPath("~"), "GalleryThumbnails", photos.GetById(id).Name);

                ImageProcessor.PhysicallyDeletePhoto(path);
                ImageProcessor.PhysicallyDeletePhoto(thumbPath);


            photos.Delete(id);
            photos.SaveChanges();

            //return RedirectToAction("Index");
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