using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.PlansViewModel;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GYMProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
       private readonly  IUnitOfWork _unitofwork;
        public PlanService(IUnitOfWork unitOfWork)
        {
           _unitofwork = unitOfWork;
        }
        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
           var plans =  await  _unitofwork.GetRepository<Plan>().GetAllAsync(ct: ct );
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
            var plan =  await _unitofwork.GetRepository<Plan>().GetByIdAsync(planid, ct: ct);
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
            var plan =  await _unitofwork.GetRepository<Plan>().GetByIdAsync(planid , ct: ct);
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
            var plan = await _unitofwork.GetRepository<Plan>().GetByIdAsync(planid, ct: cancellationToken);
            if(plan == null) return false;
            if(plan.IsActive)
            {
                plan.IsActive = false;

            }
            else
            {
                plan.IsActive = true;
            }
            _unitofwork.GetRepository<Plan>().Update(plan);
            var res = await _unitofwork.SaveChangesAsync(cancellationToken);
            return res > 0;
        }

        public async Task<bool> UpdatePlanAsync(int planid, PlanToUpdateViewModel model, CancellationToken ct = default)
        {
            var plan = await _unitofwork.GetRepository<Plan>().GetByIdAsync (planid , ct: ct);
            if (plan == null) return false;
            if (await HasActiveMemberShips(planid, ct: ct)) return false ;
            plan.DurationDays = model.DurationDays;
            plan.Description = model.Description;
            plan.Price = model.Price;
            plan.UpdatedAt = DateTime.Now;
            _unitofwork.GetRepository<Plan>().Update(plan);
            var res = await _unitofwork.SaveChangesAsync(ct);

            return res > 0;


        }
        private async  Task<bool> HasActiveMemberShips(int planid , CancellationToken ct = default)
        {
            var membershipsExist  =  await _unitofwork.GetRepository<MemberShips>() .AnyAsync(x => x.PlanId == planid && x.EndDate > DateTime.Now , ct : ct);
            return membershipsExist;
        }
    }
}
