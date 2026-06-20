using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModels.MembersVMs;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class MembersController : Controller
    {

        private readonly IMemberService memberService;
        private readonly IAttachmentService _attachmentService;

        public MembersController(IMemberService memberSer , IAttachmentService attachmentService)
        {
            memberService = memberSer;
            _attachmentService = attachmentService;
        }

        #region GET Members

        // GET: Base URL/Members/Index  => list all members

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            // Service Get all members
            var members = await memberService.GetAllMembersAsync(ct);
            return View(members);
        }

        // GET: Base URL/Members/Details/{id}  => details of a specific member
        // MemberDetails

        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            // Service Get member details by id
            var member = await memberService.GetMemberDetailsByIdAsync(id, ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
            }
            return View(member);
        }

        // GET: Base URL/Members/HealthRecord/{id}  ==> health record of a specific member
        //HealthRecordDetails

        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            // Service Get health record details by member id
            var record = await memberService.GetMemberHealthRecordAsync(id, ct);
            if (record is null)
            {
                TempData["ErrorMessage"] = "No Health Record Found  !";

                return RedirectToAction(nameof(Index));

            }
            return View(record);
        }

        // get MembersPhoto
        public async Task<IActionResult> Picture(int id)
        {
            var member = await memberService.GetMemberDetailsByIdAsync(id);
            if (member is null || string.IsNullOrWhiteSpace(member.Photo))
                return NotFound();

            var result = _attachmentService.GetFile(member.Photo, "MembersPhoto");
            if(result is null)
                return NotFound();
            return File(result.Value.stream, result.Value.contentType);
        }

        #endregion

        #region Create
        // GET: Base URL/Members/Create  => show form to create a new member
        [HttpGet]
        public IActionResult Create()
            => View();

        // POST: Base URL/Members/Create/{member}  => handle form submission to create a new member
        // CreateMember
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);

            var result = await memberService.CreateMemberAsync(model, ct);

            if (result)
                TempData["SuccessMessage"] = "Member Created Cuccessfully.";
            else
                TempData["ErrorMessage"] = "Failed To Create Member.";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit
        // GET: Base URL/Members/Edit/{id}  => show form to edit an existing member
        // EditMember
        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            // Service Get member details to update by id
            var member = await memberService.GetMemberToUpdateAsync(id, ct);

            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // POST: Base URL/Members/Edit/{member}  => handle form submission to update the member
        [HttpPost]
        public async Task<IActionResult> EditMember([FromRoute] int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await memberService.UpdateMemberAsync(id, model, ct);

            if (result)
                TempData["SuccessMessage"] = "Member Updated Successfully.";
            else
                TempData["ErrorMessage"] = "Failed To Update Member.";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete
        // GET: Base URL/Members/Delete/{id}  => show confirmation page to delete
        // DeleteConfirmed

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            // Service Get member details to delete by id
            var member = await memberService.GetMemberDetailsByIdAsync(id, ct);

            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // POST: Base URL/Members/Delete/{id}  => handle deletion of the member
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            // Service delete member by id
            var result = await memberService.DeleteMemberAsync(id, ct);

            if (result)
                TempData["SuccessMessage"] = "Member Deleted Successfully.";
            else
                TempData["ErrorMessage"] = "Failed To Delete Member.";
                
            return RedirectToAction(nameof(Index));
        }

        #endregion

    }
}
