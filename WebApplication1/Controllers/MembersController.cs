using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService service)
        {
           _memberService = service;
        }
        //GET BaseUrl/Members
        //Index - Show all Members

        public async Task<IActionResult> Index(CancellationToken ct )
        {

            var members = await  _memberService.GetAllMembersAsync(ct);
            return View(members);
          

        }

        //GET BaseUrl/Members/Details/{id} 
        //MemberDetails - Show one member detail

        public async Task<IActionResult> Details(int Id , CancellationToken ct )
        {
            var member = await _memberService.ViewMemberDetailsAsync(Id, ct);
            if (member != null)
                return View(member);
            else
                return NotFound();

        }


        // GET BaseUrl/Members/HealthRecordDetails/{id}
        //HealthRecordDetails - show one member HealthRecord

        #region CreateMember
        //Get BaseUrl/Members/Create
        //Create - show empty from
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }

        //POST BaseUrl/Members/CreateMember {Member}
        //CreateMember - Sumbit the form

        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create),model);
           var res =  await _memberService.CreateMemberAsync(model, ct);
            if (res)
                TempData["SucessMessage"] = "Member Created Sucessfully";
            else
                TempData["ErrorMessage"] = "Failed To Create Member";
                return RedirectToAction(nameof(Index));

        }

        //GET BaseUrl/Members/HealthRecordDetails/{memberId}
        // HealthRecordDetails

        public async Task<IActionResult>  HealthRecordDetails(int id , CancellationToken ct )
        {
           var healthRecord = await  _memberService.ViewHealthRecordDetailsAsync(id, ct);
            return View(healthRecord);
        }

        #endregion

        #region EditMember
        //Get BaseUrl/Members/Edit/{id}
        //Edit - show Prefilled form

        //POST BaseUrl/Members/Edit {Member}
        //Edit - Sumbit the form

        #endregion

        #region DeleteMember
        //Get BaseUrl/Members/Delete/{id}
        //Delete - show form

        //POST BaseUrl/Members/DeleteConfirmed {Member}
        //DeleteConfirmed - Sumbit the form

        #endregion




    }
}
