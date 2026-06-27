using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModels.MembershipVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.PL.Controllers
{
    public class MembershipsController : Controller
    {
        private readonly IMembershipService _membershipServ;

        public MembershipsController(IMembershipService service)
        {
            _membershipServ = service;
        }

        #region Get Memberships
        // Get :: Base URL/Memberships/Index
        public async Task<IActionResult> Index()
        {
            var memberships = await _membershipServ.GetAllMembershipsAsync();
            return View(memberships);
        }
        #endregion

        #region Create

        // GET: Base URL/Memberships/Create  => show form to create a new Membership
        // Create
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await DropDownList(ct);
            return View();
        }

        // POST: Base URL/Memberships/Create/{membership}  => handle form submission to create a new Membership
        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberShipViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await DropDownList(ct);
                return View(model);
            }

            var result = await _membershipServ.CreateMembershipAsync(model, ct);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Membership Created Cuccessfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.error;

            await DropDownList(ct);

            return View(model);

        }

        private async Task DropDownList(CancellationToken ct)
        {
            ViewBag.Plans = new SelectList(await _membershipServ.GetPlanFromDropDown(ct), "Id", "Name");
            ViewBag.Members = new SelectList(await _membershipServ.GetMemberFromDropDown(ct), "Id", "Name");
        }

        #endregion

        #region Delete

        // GET: Base URL/Memberships/Cancel/{Membership id}  => show confirmation page to delete
        // Cancel
        [HttpPost]
        public async Task<IActionResult> Cancel(int id, CancellationToken ct)
        {
            var result = await _membershipServ.DeleteActiveMembershipAsync(id, ct);

            if (result.success)
                TempData["SuccessMessage"] = "Membership Canceled Successfully.";
            else
                TempData["ErrorMessage"] = result.error;

            return RedirectToAction(nameof(Index));
        }
        
        #endregion
    }




}

