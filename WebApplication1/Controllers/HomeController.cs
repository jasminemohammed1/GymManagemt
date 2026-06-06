using System.Diagnostics;
using System.Threading.Tasks;
using GymManagement.BLL.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomePageService homePageService;

        public HomeController(ILogger<HomeController> logger , IHomePageService homePageService)
        {
            _logger = logger;
            this.homePageService = homePageService;
        }

        public async Task<IActionResult> Index(CancellationToken ct )
        {
            var res = await  homePageService.GetAnalyticAsync(ct);

            return View(res.value);
           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
