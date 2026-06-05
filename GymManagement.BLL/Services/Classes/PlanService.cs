using AutoMapper;
using GymManagement.BLL.Common;
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
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork , IMapper mapper)
        {
           _unitofwork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<Result<IEnumerable<PlanViewModel>>> GetAllPlansAsync(CancellationToken ct = default)
        {
           var plans =  await  _unitofwork.GetRepository<Plan>().GetAllAsync(ct: ct );
            var res = _mapper.Map<IEnumerable<Plan >, IEnumerable<PlanViewModel>>(plans);
            return Result<IEnumerable<PlanViewModel>>.Ok(res);
        }

        public async Task<Result<PlanViewModel>?> GetPlanByIdAsync(int planid, CancellationToken ct = default)
        {
            var plan =  await _unitofwork.GetRepository<Plan>().GetByIdAsync(planid, ct: ct);
            if (plan == null) return null;
            
            var model = _mapper.Map<Plan , PlanViewModel>(plan);
            return Result<PlanViewModel>.Ok(model);
        }

        public async  Task<Result<PlanToUpdateViewModel>?> GetPlanToUpdateAsync(int planid, CancellationToken ct = default)
        {
            var plan =  await _unitofwork.GetRepository<Plan>().GetByIdAsync(planid , ct: ct);
            if (plan == null || !plan.IsActive) return null;
            if (await HasActiveMemberShips(planid, ct: ct)) return null;
            var model = _mapper.Map<Plan , PlanToUpdateViewModel>(plan);
            return Result<PlanToUpdateViewModel>.Ok(model);
        }

        public async Task<Result> TogglePlan(int planid, CancellationToken cancellationToken = default)
        {
            var plan = await _unitofwork.GetRepository<Plan>().GetByIdAsync(planid, ct: cancellationToken);
            if (plan == null) return Result.NotFound("Plan Not Found");
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
            return res > 0 ? Result.Ok() : Result.Fail("Fail to Toggle plan");
        }

        public async Task<Result> UpdatePlanAsync(int planid, PlanToUpdateViewModel model, CancellationToken ct = default)
        {
            var plan = await _unitofwork.GetRepository<Plan>().GetByIdAsync (planid , ct: ct);
            if (plan == null) return Result.NotFound("Plan Not Found");
            if (await HasActiveMemberShips(planid, ct: ct)) return Result.Validation("Cannot Update Plan with Active Memerships") ;

            _mapper.Map(model, plan);

            plan.UpdatedAt = DateTime.Now;
            _unitofwork.GetRepository<Plan>().Update(plan);
            var res = await _unitofwork.SaveChangesAsync(ct);

            return res > 0 ? Result.Ok() : Result.Fail("Cannot update plan");


        }
        private async  Task<bool> HasActiveMemberShips(int planid , CancellationToken ct = default)
        {
            var membershipsExist  =  await _unitofwork.GetRepository<MemberShips>() .AnyAsync(x => x.PlanId == planid && x.EndDate > DateTime.Now , ct : ct);
            return membershipsExist;
        }
    }
}
