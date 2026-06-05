using AutoMapper;
using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Models.Enums;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async  Task<bool> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.StartDate >= model.EndDate) return false;
            if (model.StartDate < DateTime.Now) return false;
            if (model.Capacity < 1 || model.Capacity > 25) return false;

            var trainer =  await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if(trainer == null) return false;
            var category =  await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if (category == null) return false;
            var IsValid = Enum.TryParse<Speciality>(category.CategoryName, true, out Speciality CategorySpeciality);
            if (!IsValid || trainer.speciality != CategorySpeciality) return false;
            var session = _mapper.Map<CreateSessionViewModel, Sessions>(model);
            _unitOfWork.GetRepository<Sessions>().Add(session);
            var res = await  _unitOfWork.SaveChangesAsync(ct);
            return res > 0;

           







        }

        public async Task<IEnumerable<CetegorySelectViewModel>> GetAllCategoryForDropDownAsync(CancellationToken cancellationToken = default)
        {
            var categories =  await _unitOfWork.GetRepository<Category>().GetAllAsync(ct : cancellationToken);
            if (categories == null) return [];
            var models = _mapper.Map<IEnumerable<Category>, IEnumerable<CetegorySelectViewModel>>(categories);
            return models ;
          
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

        public  async Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var trainers =  await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            if (trainers == null) return [];
           var model =  _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(trainers);
            return model;

        }
    }
}
