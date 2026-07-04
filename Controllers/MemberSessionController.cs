using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModels.BookingVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.PL.Controllers
{
    [Authorize]
    public class MemberSessionController : Controller
    {
        // Booking controller
        private readonly IBookingService _bookingService;

        public MemberSessionController(IBookingService service)
        {
            _bookingService = service;
        }

        #region Get Bookings

        // Get :: 
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await _bookingService.GetAllSessionsAsync(ct);
            return View(sessions);
        }
        //  List Members for  Upcoming session
        public async Task<IActionResult> GetMembersForUpcomingSession(int id, CancellationToken ct)
        {
            var members = await _bookingService.GetUpcomingBookingsAsync(id,ct);
            return View(members);
        }
        //  List Members for Ongoing session
        public async Task<IActionResult> GetMembersForOngoingSessions(int id, CancellationToken ct)
        {
            var members = await _bookingService.GetOngoingBookingsAsync(id,ct);
            return View(members);
        }

        #endregion

        #region Create

        // GET:  show form to create a new Membership
        // Create
        [HttpGet]
        public async Task<IActionResult> Create(int id, CancellationToken ct)
        {
            var members = await _bookingService.GetMembersForDropDownAsync(id, ct);
            ViewBag.Members = new SelectList(members, "Id", "Name");
            ViewBag.SessionId = id;
            return View();
        }

        // POST:  handle form submission to create a new Membership
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingViewModel model, CancellationToken ct)
        {

            if (!ModelState.IsValid) return View(model);

            var result = await _bookingService.CreateBookingAsync(model);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Booking Created Cuccessfully.";
                return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });
            }

            TempData["ErrorMessage"] = result.error;

            return View(model);
        }

        #endregion

        #region Delete

        //   => show confirmation page to delete
        // Cancel
        [HttpPost]
        public async Task<IActionResult> Cancel(int memberId, int sessionId, CancellationToken ct)
        {
            var result = await _bookingService.CancelBookingAsync(memberId, sessionId, ct);

            if (result.success)
                TempData["SuccessMessage"] = "Booking Canceled Successfully.";
            else
                TempData["ErrorMessage"] = result.error;

            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = sessionId });
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> Attended(int memberId, int sessionId, CancellationToken ct)
        {
            var result = await _bookingService.MarkAttendedAsync(memberId, sessionId, ct);

            if (result.success)
                TempData["SuccessMessage"] = "Attendance recorded.";
            else
                TempData["ErrorMessage"] = result.error;

            return RedirectToAction(nameof(GetMembersForOngoingSessions), new { id = sessionId });
        }
    }
}
