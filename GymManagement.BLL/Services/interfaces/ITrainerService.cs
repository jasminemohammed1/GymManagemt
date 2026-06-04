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
        public Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default);
        public Task<bool> CreateTrainerAsync(TranierToCreateViewModel model,CancellationToken ct = default);
        public Task<TrainerDetailsViewModel ?> GetTrainerByIdAsync(int trainerid , CancellationToken ct = default);

    }
}
