using AutoMapper;
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

        public async Task<bool> CreateTrainerAsync(TranierToCreateViewModel model, CancellationToken ct = default)
        {
            var EmailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email, ct);
            var PhoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == model.Phone, ct);
            if (EmailExists || PhoneExists) return false;
            var Trainer = _mapper.Map<Trainer>(model);
            _unitOfWork.GetRepository<Trainer>().Add(Trainer);
            var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0;
        }

        public async Task<bool> DeleteTrainerAsync(int TrainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>() .GetByIdAsync(TrainerId);
            if (trainer == null) return false;

            // this checking in booking will cause problem next 

            var ShedulingSesssionsExists = await _unitOfWork.GetRepository<Sessions>().AnyAsync(x => x.TrainerId == TrainerId && x.StartDate > DateTime.Now, ct);
            if (ShedulingSesssionsExists) return false;
            _unitOfWork.GetRepository<Trainer>().Delete(trainer);
            var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers =  await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct:ct);
            if(!trainers.Any())
            {
                return [];
            }
            else
            {
                var res = _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerViewModel>>(trainers);
                return res;



           
            }

        }

        public async Task<TrainerDetailsViewModel?> GetTrainerByIdAsync(int trainerid, CancellationToken ct = default)
        {
           var trainer =  await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerid , ct );
            if (trainer == null) return null;
            var model = _mapper.Map<TrainerDetailsViewModel>(trainer);
            return model;
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpadteAsync(int trainerid, CancellationToken ct = default)
        {
           var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerid , ct );
            if (trainer == null) return null;
            var model = _mapper.Map<Trainer, TrainerToUpdateViewModel>(trainer);
            return model;

        }

        public async Task<bool> UpdateTrainerAsync(int trainerid, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerid , ct );   
            if (trainer == null) return false;
            var EmailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email && x.Id != trainerid , ct );
            var PhoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == model.Phone && x.Id != trainerid, ct);
            if (EmailExists || PhoneExists) return false;

            _mapper.Map(model, trainer);
            trainer.UpdatedAt = DateTime.Now;
            _unitOfWork.GetRepository<Trainer>().Update(trainer);
            var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0;

        }
    }

}
