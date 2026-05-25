
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

using GYMProject.Models;


namespace GYMProject.Controllers
{
    public class PlansController : Controller
    {
        private readonly IGenericRepository<Plan> _planRepo;
        public PlansController(IGenericRepository<Plan> plan)
        {
            _planRepo = plan;
        }

        public async Task<IActionResult> Index(CancellationToken ct )
        {
           var plans = await  _planRepo.GetAllAsync(ct: ct );
           
            return View(plans);
        }

        public async Task<IActionResult> Details (int id , CancellationToken ct)
        {
            var plan = await _planRepo.GetByIdAsync(id , ct: ct );
            if(plan is null)
               return  RedirectToAction(nameof(Index));

            else
                return View(plan);
            


        }
    }
}
