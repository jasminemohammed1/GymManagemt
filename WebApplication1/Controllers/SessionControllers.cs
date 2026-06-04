using GymManagement.BLL.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class SessionControllers : Controller
    {
        private readonly ISessionService _sessionService;
        public SessionControllers(ISessionService service)
        {
            _sessionService = service;
        }

        public async Task<IActionResult> Index(CancellationToken ct )
        {

            var res = await _sessionService.GetAllSessionAsync(ct);
            return View();

        }
    }
}
