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
        public TrainerService( IUnitOfWork unitOfWork) 
        {
           _unitOfWork = unitOfWork;
            
        }

        public async Task<bool> CreateTrainerAsync(TranierToCreateViewModel model, CancellationToken ct = default)
        {
            var EmailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email, ct);
            var PhoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == model.Phone, ct);
            if (EmailExists || PhoneExists) return false;
            var Trainer = new Trainer()
            {
               Phone = model.Phone,
               Email = model.Email,
               Gender = model.Gender,
               speciality = model.Speciality,
               Name = model.Name,
               DateOfBirth = model.DateOfBirth,
               Address = new Address()
               {
                   BuildeingNumber = model.BuildingNumber,
                   City = model.City,
                   Street = model.Street,
               }
            };
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
                var res = trainers.Select(x => new TrainerViewModel()
                {
                Email = x.Email,
                Id = x.Id,
                Name = x.Name,
                Specialization = x.speciality.ToString(),
                    Phone = x.Phone
                    
                    
                });
                return res;



           
            }

        }

        public async  Task<TrainerDetailsViewModel?> GetTrainerByIdAsync(int trainerid, CancellationToken ct = default)
        {
           var trainer =  await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerid , ct );
            if (trainer == null) return null;
            var model = new TrainerDetailsViewModel()
            {
                 Name = trainer.Name,
                 Phone = trainer.Phone,
                 Speciality = trainer.speciality.ToString(),
                 Address = $"{trainer.Address.BuildeingNumber} - {trainer.Address.Street} - {trainer.Address.City}",
                 DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                 Email = trainer.Email,
            };
            return model;
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpadteAsync(int trainerid, CancellationToken ct = default)
        {
           var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerid , ct );
            if (trainer == null) return null;
            var model = new TrainerToUpdateViewModel()
            {
                BuildingNumber = trainer.Address.BuildeingNumber,
                Email = trainer.Email,
                City = trainer.Address.City,
                Name = trainer.Name,
                Phone = trainer.Phone,  
                Speciality = trainer.speciality,
                Street = trainer.Address.Street

            };
            return model;

        }

        public async Task<bool> UpdateTrainerAsync(int trainerid, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerid , ct );   
            if (trainer == null) return false;
            var EmailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email && x.Id != trainerid , ct );
            var PhoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == model.Phone && x.Id != trainerid, ct);
            if (EmailExists || PhoneExists) return false;
            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.speciality = model.Speciality;
            trainer.Address.City = model.City;
            trainer.Address.BuildeingNumber = model.BuildingNumber;
            trainer.Address.Street = model.Street;
            _unitOfWork.GetRepository<Trainer>().Update(trainer);
            var res = await _unitOfWork.SaveChangesAsync(ct);
            return res > 0;

        }
    }

}
