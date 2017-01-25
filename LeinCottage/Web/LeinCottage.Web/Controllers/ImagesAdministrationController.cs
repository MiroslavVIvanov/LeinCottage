namespace LeinCottage.Web.Controllers
{
    using CustomAttributes;
    using System.Web.Mvc;

    public class ImagesAdministrationController : Controller
    {
        [BasicAuthentication("", "")]
        public ActionResult Index()
        {
            return View();
        }
    }
}