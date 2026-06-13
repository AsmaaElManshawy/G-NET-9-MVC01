using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class MembershipsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
