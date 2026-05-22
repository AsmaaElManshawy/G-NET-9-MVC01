using GymSystem.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        // Database connection
        private readonly GymDbContet context;

        public PlanController()
        {
            context = new GymDbContet();
        }

        // GET: /Plan/Index
        public async Task<IActionResult> Index()
        {
            var plans = await context.Plans.ToListAsync();
            return View(plans);
        }

        // GET: /Plan/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var plan = await context.Plans.FindAsync(id);
            if (plan == null)
                return RedirectToAction(nameof(Index));

            return View(plan);
        }
    }
}
