using GymManagement.BLL.Services.interfaces;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        //GET BaseUrl/Members/MemberDetails/{id} 
        //MemberDetails - Show one member detail


        // GET BaseUrl/Members/HealthRecordDetails/{id}
        //HealthRecordDetails - show one member HealthRecord

        #region CreateMember
        //Get BaseUrl/Members/Create
        //Create - show empty from

        //POST BaseUrl/Members/CreateMember {Member}
        //CreateMember - Sumbit the form
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
