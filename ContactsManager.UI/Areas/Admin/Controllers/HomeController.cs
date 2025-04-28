using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        [Authorize(Roles ="Admin")]
        [Route("admin/home/index")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
