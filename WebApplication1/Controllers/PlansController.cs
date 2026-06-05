
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

using GYMProject.Models;
using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.PlansViewModel;


namespace GYMProject.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanService _planService;
        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }

        public async Task<IActionResult> Index(CancellationToken ct )
        {
            var plans = await _planService.GetAllPlansAsync(ct);
           
            return View(plans.value);
        }
        [HttpGet]
        public async Task<IActionResult> Details (int id , CancellationToken ct)
        {
            var plan = await _planService.GetPlanByIdAsync(id, ct);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not Found";
                return RedirectToAction(nameof(Index));
            }

            else
                return View(plan.value);
            


        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id , CancellationToken ct)
        {
            var plan =  await _planService.GetPlanToUpdateAsync(id, ct);
            if(plan is null)
            {
                TempData["ErrorMessage"] = "Plan not Found or cannot be edited";
                return RedirectToAction(nameof(Index));
            }
            return View(plan.value);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id ,PlanToUpdateViewModel model , CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                return View(model);

            }
            else
            {
                var res = await  _planService.UpdatePlanAsync(id , model , ct);    
                if(res.Success)
                {
                    TempData["SuccessMessage"] = "Update Plan Sucessfully";

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = res.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        public async Task<IActionResult> Activate(int id , CancellationToken ct)
        {
            var res = await _planService.TogglePlan(id, ct);
            if (res.Success)
                TempData["SuccessMessage"] = "Plan Status Changes Sucessfully";
            else
                TempData["ErrorMessage"] = res.ErrorMessage;
            return RedirectToAction(nameof(Index));

        }


    }
}
