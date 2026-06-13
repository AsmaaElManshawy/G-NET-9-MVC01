using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModels.TrainersVMs;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ITrainerService trainerService;

        public TrainersController(ITrainerService trainerSer)
        {
            trainerService = trainerSer;
        }

        #region GET Trainers

        // GET: Base URL/Trainers/Index  => list all trainers

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            // Service Get all trainers
            var trainers = await trainerService.GetAllTrainersAsync(ct);
            return View(trainers);
        }

        // GET: Base URL/Trainers/Details/{id}  => details of a specific trainer
        // Details

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            // Service Get member details by id
            var trainer = await trainerService.GetTrainerDetailsByIdAsync(id, ct);
            if (trainer == null)
                TempData["ErrorMessage"] = "Trainer Not Found !";
            
            return View(trainer);
        }

        #endregion

        #region Create
        // GET: Base URL/Trainers/Create  => show form to create a new trainer
        [HttpGet]
        public IActionResult Create()
            => View();

        // POST: Base URL/Trainers/Create/{trainer}  => handle form submission to create a new trainer
        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);

            var result = await trainerService.CreateTrainerAsync(model, ct);

            if (result)
                TempData["SuccessMessage"] = "Trainer Created Cuccessfully.";
            else
                TempData["ErrorMessage"] = "Failed To Create Trainer.";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit
        // GET: Base URL/Trainers/Edit/{id}  => show form to edit an existing trainer
        // Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            // Service Get trainer details to update by id
            var trainer = await trainerService.GetTrainerToUpdateAsync(id, ct);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        // POST: Base URL/Trainers/Edit/{trainer}  => handle form submission to update the trainer
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await trainerService.UpdateTrainerAsync(id, model, ct);

            if (result)
                TempData["SuccessMessage"] = "Trainer Updated Successfully.";
            else
                TempData["ErrorMessage"] = "Failed To Update Trainer.";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete
        // GET: Base URL/Trainers/Delete/{id}  => show confirmation page to delete
        // Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            // Service Get trainer details to delete by id
            var trainer = await trainerService.GetTrainerDetailsByIdAsync(id, ct);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // POST: Base URL/Trainers/Delete/{id}  => handle deletion of the trainers
        // asp-route-id="@Context.Request.RouteValues["id"]"  => [FromRoute] more secuirty
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            // Service delete trainer by id
            var result = await trainerService.DeleteTrainerAsync(id, ct);

            if (result)
                TempData["SuccessMessage"] = "Trainer Deleted Successfully.";
            else
                TempData["ErrorMessage"] = "Failed To Delete Trainer.";

            return RedirectToAction(nameof(Index));
        }

        #endregion

    }
}
