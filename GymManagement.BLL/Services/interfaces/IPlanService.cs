using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.PlansViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.interfaces
{
    public interface IPlanService
    {
        public Task<Result<IEnumerable<PlanViewModel>>> GetAllPlansAsync(CancellationToken ct = default);
        public Task<Result<PlanViewModel> ?> GetPlanByIdAsync(int planid , CancellationToken ct = default);
        public Task<Result<PlanToUpdateViewModel> ?> GetPlanToUpdateAsync(int planid , CancellationToken ct = default);
        public Task<Result> UpdatePlanAsync(int  planid , PlanToUpdateViewModel model,CancellationToken ct = default);
        public Task<Result> TogglePlan(int planid , CancellationToken cancellationToken = default);



    }
}
