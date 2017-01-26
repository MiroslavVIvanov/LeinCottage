namespace LeinCottage.Web.Controllers
{
    using System.Web.Mvc;
    using LeinCottage.Data;
    using LeinCottage.Models;

    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index()
        {
            EfGenericRepository<Photo> photos = new EfGenericRepository<Photo>(new LeinCottageDbContext());
            var allPhotos = photos.All();
            return this.View(allPhotos);
        }
    }
}