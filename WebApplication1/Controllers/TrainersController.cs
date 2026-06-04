using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ITrainerService _trainerService;
        public TrainersController(ITrainerService service)
        {
            _trainerService = service;
        }

        public async Task<IActionResult> Index(CancellationToken ct )
        {
            var trainers = await _trainerService.GetAllTrainersAsync(ct);
            return View(trainers);
        }

        public async Task<IActionResult> Details(int id , CancellationToken ct )
        {
            var trainer = await _trainerService.GetTrainerByIdAsync(id , ct );
            if(trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not Found";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(trainer);
            }
        }
        [HttpGet]
        public async Task<IActionResult > Create(int id , CancellationToken ct )
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(int id ,TranierToCreateViewModel model , CancellationToken ct )
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var res = await _trainerService.CreateTrainerAsync(model , ct );
                if (res)
                    TempData["SuccessMessage"] = "Trainer Created SucessFully";
                else
                    TempData["ErrorMessage"] = "Faild To Create Trainer";
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {

            var member = await _trainerService.gettr(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Trainer to update not found";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(member);
            }


        }

        //POST BaseUrl/Members/Edit {Member}
        //Edit - Sumbit the form

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            // model state valid => service 
            // model state not valid => form again with same data 

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var res = await _memberService.UpdateMemberAsync(id, model, ct);
                if (res)
                {
                    TempData["SucessMessage"] = "Member updated Sucessfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed To Update Member";
                    return RedirectToAction(nameof(Index));
                }
            }

        }



    }
}
