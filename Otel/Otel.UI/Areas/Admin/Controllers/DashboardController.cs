using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Otel.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin", AuthenticationSchemes = "AdminCookie")]
    public class DashboardController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
