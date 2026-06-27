using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModels.BookingVMs;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class MemberSessionController : Controller
    {
        // Booking controller
        private readonly IBookingService _bookingService;

        public MemberSessionController(IBookingService service)
        {
            _bookingService = service;
        }

        #region Get Memberships

        // Get :: 
        public async Task<IActionResult> Index()
        {
            var sessions = await _bookingService.GetAllSessionsAsync();

            return View(sessions);
        }
        //  List Members for  Upcoming session
        public async Task<IActionResult> GetMembersForUpcomingSession(int sessionId)
        {
            var members = await _bookingService.GetMembersForSessionAsync(sessionId);

            return View(members);
        }
        //  List Members for Ongoing session
        public async Task<IActionResult> GetMembersForOngoingSessions(int sessionId)
        {
            var members = await _bookingService.GetMembersForSessionAsync(sessionId);

            return View(members);
        }

        #endregion

        #region Create

        // GET:  show form to create a new Membership
        // Create
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
            => View();

        // POST:  handle form submission to create a new Membership
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingViewModel model, CancellationToken ct)
        {

            if (!ModelState.IsValid) return View(model);

            var result = await _bookingService.CreateBookingAsync(model);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Booking Created Cuccessfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.error;

            return View(model);
        }

        #endregion

        #region Delete

        // GET: Base URL/Memberships/Delete/{Membership id}  => show confirmation page to delete
        // Cancel
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var result =
                await _bookingService.CancelBookingAsync(id);

            if (result.success)
                TempData["SuccessMessage"] = "Membership Canceled Successfully.";
            else
                TempData["ErrorMessage"] = result.error;

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
