using Microsoft.AspNetCore.Mvc;

namespace SchoolApp.Controllers
{
    public class GlobalController : Controller
    {
        public IActionResult NotFound()
        {
            return View();
        }
    }
}
