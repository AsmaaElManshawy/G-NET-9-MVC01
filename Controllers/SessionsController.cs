using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModels.SessionsVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GymSystem.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionsService _sessionServ;

        public SessionsController(ISessionsService service)
        {
            _sessionServ = service;
        }

        #region Get Sessions
        // Get :: Base URL/Sessions/Index
        public async Task<IActionResult> Index()
        {
            var sessions = await _sessionServ.GetAllSessionsAsync();
            return View(sessions);
        }

        // GET :: Base URL/Sessions/Details/{id}
        // Details
        [HttpGet]
        public async Task<IActionResult> Details(int id , CancellationToken ct)
        {
            var result = await _sessionServ.GetSessionByIdAsync(id,ct);
            if (result.success) return View(result.value);
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        #region Create

        // GET: Base URL/Sessions/Create  => show form to create a new Session
        // Create
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await DropDownList(ct);
            return View();
        }

        // POST: Base URL/Sessions/Create/{session}  => handle form submission to create a new Session
        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) 
            {
                await DropDownList(ct);
                return View(model); 
            }

            var result = await _sessionServ.CreateSessionAsync(model, ct);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Created Cuccessfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.error;

            await DropDownList(ct);

            return View(model);

        }

        private async Task DropDownList(CancellationToken ct) 
        {
            ViewBag.Trainers = new SelectList(await _sessionServ.GetTrainerFromDropDown(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionServ.GetCategoryFromDropDown(ct), "Id", "CategoryName");
        }
        #endregion

        #region Edit
        
        // Get: Base URL/Sessions/Edit/{id}  => create Edit action
        // Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _sessionServ.GetSessionToUpdate(id, ct);

            if (result.success)
            {
                await DropDownList(ct);
                return View(result.value);
            }
            else 
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Base URL/Sessions/Edit/{session}  => handle form submission to create a new Session
        [HttpPost]
        public async Task<IActionResult> Edit( int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await DropDownList(ct);
                return View(model);
            }

            var result = await _sessionServ.UpdateSessionAsync(id,model, ct);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Updated Cuccessfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.error;

            await DropDownList(ct);

            return View(model);
        }

        #endregion

        #region Delete
        
        // GET: Base URL/Sessions/Delete/{id}  => show confirmation page to delete
        // Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            // Service Get session details to delete by id
            var result = await _sessionServ.GetSessionByIdAsync(id, ct);

            if (!result.success)
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // POST: Base URL/Sessions/Delete/{id}  => handle deletion of the session
        // DeleteConfirmed
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            // Service delete session by id
            var result = await _sessionServ.DeleteSessionAsync(id, ct);

            if (result.success)
                TempData["SuccessMessage"] = "Sesssion Deleted Successfully.";
            else
                TempData["ErrorMessage"] = result.error;

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
