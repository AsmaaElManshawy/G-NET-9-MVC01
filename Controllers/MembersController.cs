using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModels;
using GymManagment.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class MembersController : Controller
    {

        private readonly IMemberService memberService;

        public MembersController(IMemberService memberSer)
        {
            memberService = memberSer;
        }

        #region GET Members

        // GET: Base URL/Members/Index  => list all members

        public  async Task<IActionResult> Index(CancellationToken ct)
        {
            // Service Get all members
             var members = await memberService.GetAllMembersAsync(ct);
            return View(members);
        }

        // GET: Base URL/Members/Details/{id}  => details of a specific member
        // MemberDetails


        // GET: Base URL/Members/HealthRecord/{id}  ==> health record of a specific member
        //HealthRecordDetails

        #endregion

        #region Create
        // GET: Base URL/Members/Create  => show form to create a new member
        [HttpGet]
        public IActionResult Create()
            => View();

        // POST: Base URL/Members/Create/{member}  => handle form submission to create a new member
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid) return View( nameof(Create) ,model);

            var result = await memberService.CreateMemberAsync(model, ct);

            if (result)
                TempData["SuccessMessage"] = "Member created successfully.";
            else
                TempData["ErrorMessage"] = "Failed to create member. Please try again.";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit
        // GET: Base URL/Members/Edit/{id}  => show form to edit an existing member
        // EditMember

        // POST: Base URL/Members/Edit/{member}  => handle form submission to update the member
        #endregion

        #region Delete
        // GET: Base URL/Members/Delete/{id}  => show confirmation page to delete
        // Delete

        // POST: Base URL/Members/Delete/{id}  => handle deletion of the member
        #endregion

    }
}
