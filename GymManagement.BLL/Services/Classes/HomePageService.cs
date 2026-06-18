using AutoMapper.Execution;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.HomePageViewModel;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class HomePageService : IHomePageService
    {
        private readonly IUnitOfWork unitOfWork;

        public HomePageService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<HomePageViewModel>> GetAnalyticAsync(CancellationToken ct = default)
        {
            var date = DateTime.Now;
            var MemberCount = await unitOfWork.GetRepository<DAL.Models.Member>().CountAsync(ct: ct);
            var ActiveMemberCount = await unitOfWork.GetRepository<MemberShips>().CountAsync(x => x.EndDate > date, ct);
            var TrainerCount = await unitOfWork.GetRepository<Trainer>().CountAsync(ct:ct);
            var CompletedSessionsCount = await unitOfWork.GetRepository<Sessions>().CountAsync(x => x.EndDate < date, ct);
            var OnGoingSessionsCount = await unitOfWork.GetRepository<Sessions>().CountAsync(x => x.StartDate <= date && x.EndDate > date, ct);
            var UpComingSessionsCount = await unitOfWork.GetRepository<Sessions>().CountAsync(x => x.EndDate > date , ct);
            var model = new HomePageViewModel()
            {
                CountActiveMember = ActiveMemberCount,
                CountAllMember = MemberCount,
                CountAllTrainers = TrainerCount,
                CountCompletedSessions = CompletedSessionsCount,
                CountOnGoingSessions = OnGoingSessionsCount,
                CountUpComingSesions = UpComingSessionsCount,
            };
            return Result<HomePageViewModel>.Ok(model);
           

        }
    }
}
