using GymManagement.BLL.Common;
using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.HomePageViewModel;
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
           
            var MemberCount =  await unitOfWork.MemberRepository.GetMemberCount(ct);
            var ActiveMemberCount = await unitOfWork.MemberRepository.GetMemberCountWithActiveMemberShips(ct);
            var TrainerCount = await unitOfWork.TrainerRepository.GetTrainerCount(ct);
            var CompletedSessionsCount = await unitOfWork.SessionRepository.GetCompletedSessionsCount(ct);
            var OnGoingSessionsCount = await unitOfWork.SessionRepository.GetOnGoingSessionsCount(ct);
            var UpComingSessionsCount = await unitOfWork.SessionRepository.GetUpComingSessionsCount(ct);
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
