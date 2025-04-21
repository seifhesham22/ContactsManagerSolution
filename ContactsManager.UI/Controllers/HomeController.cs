using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CRUDEXAMPLE.Controllers
{
    public class HomeController : Controller
    {
        [Route("Error")]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature? pathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (pathFeature != null && pathFeature.Error != null) ViewBag.Error = pathFeature.Error.Message;
            return View(); //Views/Shared/Error
        }
    }
}
