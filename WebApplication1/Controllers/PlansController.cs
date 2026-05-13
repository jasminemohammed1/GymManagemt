using GYMProject.DBContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace GYMProject.Controllers
{
    public class PlansController : Controller
    {
        private readonly GymDBContext gymDBContext;
        public PlansController()
        {
            gymDBContext = new GymDBContext();
        }
        public async Task<IActionResult> Index()
        {
            var plans = await gymDBContext.Plans.ToListAsync();
            return View(plans);
        }

        public async Task<IActionResult> Details (int id)
        {
            var plan = await gymDBContext.Plans.FindAsync( id);
            if(plan is null)
               return  RedirectToAction(nameof(Index));

            else
                return View(plan);
            


        }
    }
}
