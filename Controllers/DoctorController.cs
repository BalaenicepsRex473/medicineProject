using Microsoft.AspNetCore.Mvc;

namespace scrubsAPI.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
