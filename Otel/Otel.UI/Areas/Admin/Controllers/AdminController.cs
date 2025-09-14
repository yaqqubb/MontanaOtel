using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Otel.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
   // [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class AdminController : Controller
    {
        public AdminController() { }
    }
}
