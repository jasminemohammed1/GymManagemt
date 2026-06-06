using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;
        public SessionsController(ISessionService service)
        {
            _sessionService = service;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {

            var res = await _sessionService.GetAllSessionAsync(ct);
            return View(res);

        }

        #region CreateSession
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownList(ct);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownList(ct);

                return View(model);
            }
            var res = await _sessionService.CreateSessionAsync(model, ct);
            if (res.Success)
            {
                TempData["SuccessMessage"] = "Session Created Sucessfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = res.ErrorMessage;
                await PopulateDropDownList(ct);
                return View(model);

            }
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Details(int id , CancellationToken ct )
        {
            var session = await  _sessionService.GetSessionByIdAsync(id);
            if(session.Sucess)
            {
                return View(session.value);
            }
            else
            {
                TempData["ErrorMessage"] = session.ErrorMessage;
                return RedirectToAction(nameof(Index));

            }
        }









        private async Task PopulateDropDownList(CancellationToken ct)
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetAllTrainersForDropDownAsync(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetAllCategoryForDropDownAsync(ct), "Id", "CategoryName");
        }

    }
}