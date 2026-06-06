using AutoMapper;
using GymManagement.BLL.Common;
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

        public async  Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.StartDate >= model.EndDate) return Result.Validation("StartDate must be < EndDate");
            if (model.StartDate < DateTime.Now) return Result.Validation("StartDate must be in the Future");
            if (model.Capacity < 1 || model.Capacity > 25) return Result.Validation("Capacity must be between 1 & 25");

            var trainer =  await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer == null) return Result.NotFound("Trainer not Found");
            var category =  await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if (category == null) return Result.NotFound("Category Not Found");
            var IsValid = Enum.TryParse<Speciality>(category.CategoryName, true, out Speciality CategorySpeciality);
            if (!IsValid || trainer.speciality != CategorySpeciality) return Result.Validation("Category Must Match Trainer Speciality");
            var session = _mapper.Map<CreateSessionViewModel, Sessions>(model);
            _unitOfWork.GetRepository<Sessions>().Add(session);
            var res = await  _unitOfWork.SaveChangesAsync(ct);
            return res > 0 ? Result.Ok() : Result.Fail("Cannot Create This Session");

           







        }

        public async Task<Result> DeleteSessionAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Sessions>().GetByIdAsync(sessionId, ct);
            if (session == null) return Result.NotFound("Session Not Found");
            if (session.EndDate > DateTime.Now) return Result.Validation("Cannot Delete OnGoing  Or UpComing Session");
            var BookingCount = await _unitOfWork.SessionRepository.GetBookedSlotsAsync(sessionId, ct);
            if (BookingCount > 0) return Result.Validation("Cannot Delete Session With Booking on it");
            _unitOfWork.GetRepository<Sessions>().Delete(session);
            var res = await  _unitOfWork.SaveChangesAsync(ct);
            return res > 0 ? Result.Ok() : Result.Fail("Fail To Delete Session");


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

        

        

        public async Task<Result<SessionViewModel>> GetSessionByIdAsync(int sessionId, CancellationToken ct = default)
        {
           var session = await  _unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategory(sessionId, ct );
            if (session == null) return Result<SessionViewModel>.NotFound("Session Not Found");
            var mappedSession = _mapper.Map<Sessions,SessionViewModel>(session);
            mappedSession.AvailableSlots = mappedSession.Capacity - await _unitOfWork.SessionRepository.GetBookedSlotsAsync(sessionId, ct);

            return Result<SessionViewModel>.Ok(mappedSession);

        }

        public  async Task<Result<SessionToUpdateViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct)
        {
            var session = await _unitOfWork.GetRepository<Sessions>().GetByIdAsync(sessionId, ct);
            if (session == null) return Result<SessionToUpdateViewModel>.NotFound("Session Not Found");
            var mappedSession = _mapper.Map<Sessions, SessionToUpdateViewModel>(session);
            return Result<SessionToUpdateViewModel>.Ok(mappedSession);
        }

        public async Task<Result> UpdateSessionAsync(int sessionId, SessionToUpdateViewModel model, CancellationToken ct = default)
        {
            var session = await  _unitOfWork.GetRepository<Sessions>().GetByIdAsync(sessionId, ct);
            if (session == null) return Result.NotFound("Session Not Found");
            // check on session to be updatable or not 

            if (session.StartDate < DateTime.Now) return Result.Validation("Cannot Update on Completed or OngoingSession");
            var BookingCount =  await _unitOfWork.SessionRepository.GetBookedSlotsAsync(sessionId, ct);
            if (BookingCount > 0) return Result.Validation("Cannot Update Session with One Booking on it ");

            // check on the model as create 

            if (model.StartDate >= model.EndDate) return Result.Validation("StartDate must be < EndDate");
            if (model.StartDate < DateTime.Now) return Result.Validation("StartDate Must Be in Future");
            var trainer = await  _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId);
            if (trainer == null) return Result.NotFound("Trainer Not Found");
            var IsValid = Enum.TryParse<Speciality>(session.Category?.CategoryName, true, out Speciality CategorySpeciality);
            if (!IsValid || trainer.speciality!= CategorySpeciality) return Result.Validation("Trainer Speciality must match Category");

            var mappedSesion = _mapper.Map(model, session);
            mappedSesion.UpdatedAt = DateTime.Now;
            _unitOfWork.GetRepository<Sessions>().Update(mappedSesion);
           var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0 ? Result.Ok() : Result.Fail("Cannot Update Session");




        }
    }
}
