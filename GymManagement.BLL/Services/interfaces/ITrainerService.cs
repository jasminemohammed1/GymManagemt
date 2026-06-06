using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.interfaces
{
    public interface ITrainerService
    {
        public Task<Result<IEnumerable<TrainerViewModel>>> GetAllTrainersAsync(CancellationToken ct = default);
        public Task<Result> CreateTrainerAsync(TranierToCreateViewModel model,CancellationToken ct = default);
        public Task<Result<TrainerDetailsViewModel ?>> GetTrainerByIdAsync(int trainerid , CancellationToken ct = default);
        public Task<Result<TrainerToUpdateViewModel?>> GetTrainerToUpadteAsync(int trainerid, CancellationToken ct = default);
        public Task<Result> UpdateTrainerAsync(int trainerid, TrainerToUpdateViewModel model, CancellationToken ct = default);
        public Task<Result> DeleteTrainerAsync(int TrainerId, CancellationToken ct = default);
      

    }
}
