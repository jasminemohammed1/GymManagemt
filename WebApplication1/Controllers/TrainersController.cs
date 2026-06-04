using GymManagement.BLL.Services.interfaces;
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

    }
}
