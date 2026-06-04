using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public async Task<IEnumerable<SessionViewModel> ?> GetAllSessionAsync(CancellationToken ct = default)
        {
            var res = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerandCategory(ct);
            if(res == null ||!res.Any())
            {
                return null;
            }
            var res2 = res.Select(x => new SessionViewModel()
            {
                Id = x.Id,
                Capacity    = x.Capacity,
                Category    = x.Category.CategoryName ,
                TrainerName = x.Trainer.Name,
                Description = x.Description,
                EndDate = x.EndDate,
                StartDate = x.StartDate ,
               
            });
            foreach(var item in res2)
            {
                item.AvailableSlots = item.Capacity - await _unitOfWork.SessionRepository.GetBookedSlotsAsync(item.Id ,ct );
            }
            return res2;
        }
    }
}
