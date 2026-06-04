using GymManagement.BLL.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;
        public SessionsController(ISessionService service)
        {
            _sessionService = service;
        }

        public async Task<IActionResult> Index(CancellationToken ct )
        {

            var res = await _sessionService.GetAllSessionAsync(ct);
            return View(res);

        }
    }
}
