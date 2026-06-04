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
        public Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);
        public Task<PlanViewModel ?> GetPlanByIdAsync(int planid , CancellationToken ct = default);
        public Task<PlanToUpdateViewModel ?> GetPlanToUpdateAsync(int planid , CancellationToken ct = default);
        public Task<bool> UpdatePlanAsync(int  planid , PlanToUpdateViewModel model,CancellationToken ct = default);
        public Task<bool> TogglePlan(int planid , CancellationToken cancellationToken = default);



    }
}
