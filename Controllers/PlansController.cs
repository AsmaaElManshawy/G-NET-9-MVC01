
using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModels.PlansVMs;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class PlansController : Controller
    {
        // Database connection
        private readonly IPlanService planService;
        public PlansController(IPlanService planSer)
        {
            planService = planSer;
        }

        #region Get Plans

        // GET: Base URL/Plans/Index  => list all plans
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await planService.GetAllPlansAsync(ct);
            return View(plans);
        }

        
        // GET: Base URL/Plans/Details/{id}  => details of a specific plan
        public async Task<IActionResult> Details(int id , CancellationToken ct)
        {
            var plan = await planService.GetPlanDetailsByIdAsync(id , ct);
            if (plan == null)
                TempData["ErrorMessage"] = "Plan Not Found !";

            return View(plan);
        }

        #endregion


        #region Edit
        // GET: Base URL/Plans/Edit/{id}  => show form to edit an existing plan
        // Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            // Service Get plan details to update by id
            var plan = await planService.GetPlanToUpdateAsync(id, ct);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "plan Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        // POST: Base URL/Plans/Edit/{member}  => handle form submission to update the plan
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, UpdatePlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await planService.UpdatePlanAsync(id, model, ct);

            if (result)
                TempData["SuccessMessage"] = "Plan Updated Successfully.";
            else
                TempData["ErrorMessage"] = "Failed To Update Plan.";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        // Soft delete for a plan
        [HttpPost]
        public async Task<IActionResult> Activate(int id, CancellationToken ct)
        {
            string result = await planService.SoftDeletePlanAsync(id, ct);

            if (result == "false")
                TempData["ErrorMessage"] = "Plan Not Found ! Or Has Active Memberships.";
            else if (result == "DActivated")
                TempData["SuccessMessage"] = "Plan DActivated Successfully";
            else if (result == "Activated")
                TempData["SuccessMessage"] = "Plan Activated Successfully";

            return RedirectToAction(nameof(Index));
        }

    }
}
