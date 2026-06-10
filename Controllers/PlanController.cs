using GymManagment.DAL.Models;
using GymManagment.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class PlanController : Controller
    {
        // Database connection
        private readonly IGenericRepository<Plan> planRepository;


        public PlanController(IGenericRepository<Plan> planRepo)
        {
            planRepository = planRepo;
        }
        // GET: /Plan/Index
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await planRepository.GetAllAsync(ct : ct); // pass by name
            return View(plans);
        }

        // GET: /Plan/Details/{id}
        public async Task<IActionResult> Details(int id , CancellationToken ct)
        {
            var plan = await planRepository.GetByIdAsync(id , ct);
            if (plan == null)
                return RedirectToAction(nameof(Index));

            return View(plan);
        }
    }
}
