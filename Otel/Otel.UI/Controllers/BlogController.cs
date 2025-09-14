using Microsoft.AspNetCore.Mvc;

namespace Otel.UI.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
