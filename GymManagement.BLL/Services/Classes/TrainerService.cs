using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService( IUnitOfWork unitOfWork , IMapper mapper) 
        {
           _unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<Result> CreateTrainerAsync(TranierToCreateViewModel model, CancellationToken ct = default)
        {
            var EmailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email, ct);
            var PhoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == model.Phone, ct);
            if (EmailExists || PhoneExists) return Result.Validation("Email or Phone Exists before");
            var Trainer = _mapper.Map<Trainer>(model);
            _unitOfWork.GetRepository<Trainer>().Add(Trainer);
            var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0 ? Result.Ok() : Result.Fail("Cannot Create Trainer");
        }

        public async Task<Result> DeleteTrainerAsync(int TrainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>() .GetByIdAsync(TrainerId);
            if (trainer == null) return Result.NotFound("Trainer Not Found");

            // this checking in booking will cause problem next 

            var ShedulingSesssionsExists = await _unitOfWork.GetRepository<Sessions>().AnyAsync(x => x.TrainerId == TrainerId && x.StartDate > DateTime.Now, ct);
            if (ShedulingSesssionsExists) return Result.Validation("Cannot Delete Trainer With Scheduling Sessions");
            _unitOfWork.GetRepository<Trainer>().Delete(trainer);
            var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0 ? Result.Ok() : Result.Fail("Fail To  Delete Trainer");
        }

        public async Task<Result<IEnumerable<TrainerViewModel>>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers =  await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct:ct);
            if(!trainers.Any())
            {
                return Result<IEnumerable<TrainerViewModel>>.Ok([]);
            }
            else
            {
                var res = _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerViewModel>>(trainers);
                return Result<IEnumerable<TrainerViewModel>>.Ok(res);



           
            }

        }

        public async Task<Result<TrainerDetailsViewModel>?> GetTrainerByIdAsync(int trainerid, CancellationToken ct = default)
        {
           var trainer =  await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerid , ct );
            if (trainer == null) return null;
            var model = _mapper.Map<TrainerDetailsViewModel>(trainer);
            return Result<TrainerDetailsViewModel>.Ok(model);
        }

        public async Task<Result<TrainerToUpdateViewModel>?> GetTrainerToUpadteAsync(int trainerid, CancellationToken ct = default)
        {
           var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerid , ct );
            if (trainer == null) return null;
            var model = _mapper.Map<Trainer, TrainerToUpdateViewModel>(trainer);
            return Result <TrainerToUpdateViewModel >.Ok(model);

        }

        public async Task<Result> UpdateTrainerAsync(int trainerid, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerid , ct );   
            if (trainer == null) return Result.Validation("Trainer not Found");
            var EmailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email && x.Id != trainerid , ct );
            var PhoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == model.Phone && x.Id != trainerid, ct);
            if (EmailExists || PhoneExists) return Result.Validation("Phone Or Email Exists Before");

            _mapper.Map(model, trainer);
            trainer.UpdatedAt = DateTime.Now;
            _unitOfWork.GetRepository<Trainer>().Update(trainer);
            var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0 ? Result.Ok() : Result.Fail("Fail To Update Trainer");

        }
    }

}
