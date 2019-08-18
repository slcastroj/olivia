using Microsoft.AspNetCore.Mvc;

namespace Olivia.Web.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}