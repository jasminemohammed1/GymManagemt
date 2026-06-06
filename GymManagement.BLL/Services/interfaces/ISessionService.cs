using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.interfaces
{
    public interface ISessionService
    {
        public Task<IEnumerable<SessionViewModel>> GetAllSessionAsync(CancellationToken ct = default);
        public Task<Result> CreateSessionAsync(CreateSessionViewModel model , CancellationToken ct = default);
        public Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainersForDropDownAsync(CancellationToken ct = default);
        public Task<IEnumerable<CetegorySelectViewModel>>GetAllCategoryForDropDownAsync(CancellationToken cancellationToken = default);
        public Task<Result<SessionViewModel>> GetSessionByIdAsync(int sessionId , CancellationToken ct = default);

        public Task<Result<SessionToUpdateViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct);
        public Task<Result> UpdateSessionAsync(int sessionId , SessionToUpdateViewModel model , CancellationToken ct = default);
    }
}
