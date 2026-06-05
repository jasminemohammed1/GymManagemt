using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Http.Metadata;
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

        #region CreateSession
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> Create(CreateSessionViewModel model , CancellationToken ct )
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var res = await _sessionService.CreateSessionAsync(model, ct);
            if(res)
            {
                TempData["SuccessMessage"] = "Session Created Sucessfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Faild To Create Session";
                return View(model);

            }
        }
        #endregion
    }
}
