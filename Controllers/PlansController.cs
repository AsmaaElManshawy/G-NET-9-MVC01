
using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModels.PlansVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    [Authorize]
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

        [HttpGet]
        // GET: Base URL/Plans/Details/{id}  => details of a specific plan
        public async Task<IActionResult> Details(int id , CancellationToken ct)
        {
            var plan = await planService.GetPlanDetailsByIdAsync(id , ct);
            if (!plan.success)
                TempData["ErrorMessage"] = plan.error;

            return View(plan.value);
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

            if (!plan.success)
            {
                TempData["ErrorMessage"] = plan.error;
                return RedirectToAction(nameof(Index));
            }
            return View(plan.value);
        }

        // POST: Base URL/Plans/Edit/{member}  => handle form submission to update the plan
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, UpdatePlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await planService.UpdatePlanAsync(id, model, ct);

            if (result.success)
                TempData["SuccessMessage"] = "Plan Updated Successfully.";
            else
                TempData["ErrorMessage"] = result.error;

            return RedirectToAction(nameof(Index));
        }

        #endregion

        // Soft delete for a plan
        [HttpPost]
        public async Task<IActionResult> Activate(int id, CancellationToken ct)
        {
            var result = await planService.SoftDeletePlanAsync(id, ct);

            if (!result.success)
                TempData["ErrorMessage"] = result.error;
            else if (result.value == "DActivated")
                TempData["SuccessMessage"] = "Plan DActivated Successfully";
            else if (result.value == "Activated")
                TempData["SuccessMessage"] = "Plan Activated Successfully";

            return RedirectToAction(nameof(Index));
        }

    }
}
