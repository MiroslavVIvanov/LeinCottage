namespace LeinCottage.Web.Controllers
{
    using System.Web.Mvc;

    public class ContactsController : Controller
    {
        // GET: Contacts
        public ActionResult Index()
        {
            return this.View();
        }
    }
}