namespace LeinCottage.Web.Controllers
{
    using LeinCottage.Data;
    using LeinCottage.Models;
    using System.Linq;
    using System.Web.Mvc;

    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index()
        {
            EfGenericRepository<Photo> photos = new EfGenericRepository<Photo>(new LeinCottageDbContext());
            var allPhotos = photos.All();
            return View(allPhotos);
        }
    }
}