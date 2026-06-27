using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class UserController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
