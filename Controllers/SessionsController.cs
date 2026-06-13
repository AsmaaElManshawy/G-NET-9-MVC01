using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class SessionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
