using GymManagement.BLL.Services.Attachments;
using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IAttachmentService attachmentService;

        public MembersController(IMemberService service , IAttachmentService attachmentService)
        {
            _memberService = service;
            this.attachmentService = attachmentService;
        }




        [HttpGet]
        public async  Task<IActionResult> Picture(int id )
        {
            var res =  await _memberService.ViewMemberDetailsAsync(id);
           if(res.value == null || string.IsNullOrWhiteSpace(res.value.Photo)) return NotFound();

            var res2 = attachmentService.GetFile(res.value.Photo, "MembersPhoto");

            if(res2 == null) return NotFound();
            return File(res2.value.stream, res2.value.ContentType);
        }
        //GET BaseUrl/Members
        //Index - Show all Members

        public async Task<IActionResult> Index(CancellationToken ct)
        {

            var members = await _memberService.GetAllMembersAsync(ct);
            return View(members.value);


        }

        //GET BaseUrl/Members/MemberDetails/{id} 
        //MemberDetails - Show one member detail


        public async  Task<IActionResult> MemberDetails(int id , CancellationToken ct )
        {
              //if it is null => index with message 
              //else => view 
              var memberDetails = await _memberService.ViewMemberDetailsAsync(id , ct );
              if(memberDetails is  null )
            {
                TempData["ErrorMessage"] = "Member not found";
                   
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(memberDetails.value);
            }

        }







        


        // GET BaseUrl/Members/HealthRecordDetails/{id}
        //HealthRecordDetails - show one member HealthRecord

        public async Task<IActionResult> HealthRecordDetails( int id , CancellationToken ct )
        {
            var healthrecord =  await _memberService.ViewMemberHealthRecordAsync(id , ct );
            // if found => view 
            if(healthrecord is null )
            {
                TempData["ErrorMessage"] = "Member not Found";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(healthrecord.value);
            }
            // else => go to index with message
        }

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

        //[HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create), model);
            var res = await _memberService.CreateMemberAsync(model, ct);
            if (res.Success)
                TempData["SucessMessage"] = "Member Created Sucessfully";
            else
                TempData["ErrorMessage"] = res.ErrorMessage;
            return RedirectToAction(nameof(Index));

        }

       
      

        #endregion

        #region EditMember
        //Get BaseUrl/Members/Edit/{id}
        //Edit - show Prefilled form
        [HttpGet]
       public async Task<IActionResult> EditMember(int id , CancellationToken ct)
        {

            var member = await  _memberService.GetMemberToUpdateAsync(id , ct);
            if(member is null)
            {
                TempData["ErrorMessage"] = "Member to update not found";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(member.value);
            }


        }

        //POST BaseUrl/Members/Edit {Member}
        //Edit - Sumbit the form

        [HttpPost]
        public async  Task<IActionResult> EditMember(int id , MemberToUpdateViewModel model, CancellationToken ct)
        {
            // model state valid => service 
            // model state not valid => form again with same data 

            if(!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var res =  await _memberService.UpdateMemberAsync(id , model, ct);
                if(res.Success)
                {
                    TempData["SucessMessage"] = "Member updated Sucessfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = res.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
            }

        }





        #endregion

        #region DeleteMember
        //Get BaseUrl/Members/Delete/{id}
        //Delete - show form

        [HttpGet]
        public async  Task<IActionResult> Delete(int id , CancellationToken ct )
        {
            //if member not found => index 
            //else => view 
            var member =  await _memberService.ViewMemberDetailsAsync(id , ct);
            if(member is null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof (Index));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute]int id , CancellationToken ct )
        {
            var res = await  _memberService.DeleteMemberAsync(id , ct);    
            if(res.Success)

            {
                TempData["SucessMessage"] = "Member Delete Sucessfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = res.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }

       

        #endregion




    }

}
