namespace LeinCottage.Web.Controllers
{
    using System.Web.Mvc;
    using LeinCottage.Data;
    using LeinCottage.Models;
    using LocalData;

    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index()
        {
            var photos = JsonPhotoRepository<Photo>.Instance;
            var allPhotos = photos.All();
            return this.View(allPhotos);
        }
    }
}