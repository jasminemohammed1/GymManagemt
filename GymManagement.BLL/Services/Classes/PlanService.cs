using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.PlansViewModel;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GYMProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IGenericRepository<Plan> _PlanRepo;
        private readonly IGenericRepository<MemberShips> _membershipsRepo;
        public PlanService(IGenericRepository<Plan> planrepo , IGenericRepository<MemberShips> _memberrepo)
        {
            _PlanRepo = planrepo;
            _membershipsRepo = _memberrepo;
        }
        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
           var plans =  await  _PlanRepo.GetAllAsync(ct: ct );
            return plans.Select(x => new PlanViewModel()
            {
                Name =x.Name ,
                Description =x.Description ,
                Price =x.Price ,
                Duration = x.DurationDays,
                IsActive = x.IsActive ,
                Id = x.Id ,
            });
        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int planid, CancellationToken ct = default)
        {
            var plan =  await _PlanRepo.GetByIdAsync(planid, ct: ct);
            if (plan == null) return null;
            
            var model = new PlanViewModel()
            {
                Name = plan.Name ,
                Price = plan.Price ,
                Duration = plan.DurationDays,
                IsActive = plan.IsActive ,

            };
            return model;
        }

        public async  Task<PlanToUpdateViewModel?> GetPlanToUpdateAsync(int planid, CancellationToken ct = default)
        {
            var plan =  await _PlanRepo.GetByIdAsync(planid , ct: ct);
            if (plan == null || !plan.IsActive) return null;
            if (await HasActiveMemberShips(planid, ct: ct)) return null;
            var model = new PlanToUpdateViewModel()
            {
                Name = plan.Name ,
                Description = plan.Description ,
                Price = plan.Price ,
                DurationDays = plan.DurationDays,
                


            };
            return model;
        }

        public async Task<bool> TogglePlan(int planid, CancellationToken cancellationToken = default)
        {
            var plan = await  _PlanRepo.GetByIdAsync(planid, ct: cancellationToken);
            if(plan == null) return false;
            if(plan.IsActive)
            {
                plan.IsActive = false;

            }
            else
            {
                plan.IsActive = true;
            }
            var res = await  _PlanRepo.UpdateAsync(plan, cancellationToken);
            return res > 0;
        }

        public async Task<bool> UpdatePlanAsync(int planid, PlanToUpdateViewModel model, CancellationToken ct = default)
        {
            var plan = await _PlanRepo.GetByIdAsync (planid , ct: ct);
            if (plan == null) return false;
            if (await HasActiveMemberShips(planid, ct: ct)) return false ;
            plan.DurationDays = model.DurationDays;
            plan.Description = model.Description;
            plan.Price = model.Price;
            plan.UpdatedAt = DateTime.Now;
            var res = await  _PlanRepo.UpdateAsync(plan, ct: ct);
            return res > 0;


        }
        private async  Task<bool> HasActiveMemberShips(int planid , CancellationToken ct = default)
        {
            var membershipsExist  =  await _membershipsRepo.AnyAsync(x => x.PlanId == planid && x.EndDate > DateTime.Now , ct : ct);
            return membershipsExist;
        }
    }
}
