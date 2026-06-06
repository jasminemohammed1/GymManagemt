using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.interfaces
{
    public interface ISessionRepository 
    {
        public Task<IEnumerable<Sessions>> GetAllSessionsWithTrainerandCategory(CancellationToken ct);
        public Task<int> GetBookedSlotsAsync(int Id ,CancellationToken ct);
        public Task<Sessions?> GetSessionByIdWithTrainerAndCategory(int sessionId , CancellationToken ct);
        public Task<int> GetOnGoingSessionsCount(CancellationToken ct);
        public Task<int> GetUpComingSessionsCount(CancellationToken ct);
        public Task<int> GetCompletedSessionsCount(CancellationToken ct);

    }
}
