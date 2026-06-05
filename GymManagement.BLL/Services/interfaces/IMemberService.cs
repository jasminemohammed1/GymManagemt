using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.interfaces
{
    public interface IMemberService
    {
        public Task<Result<IEnumerable<MemberViewModel>>> GetAllMembersAsync(CancellationToken ct = default);
        public Task<Result> CreateMemberAsync(CreateMemberViewModel createMemberViewModel , CancellationToken ct = default);
        public Task<Result<MemberViewModel?>> ViewMemberDetailsAsync(int memberId, CancellationToken ct = default);
        public Task<Result<HealthRecordViewModel ?> >ViewMemberHealthRecordAsync(int memberId, CancellationToken ct = default);
        
        public Task<Result<MemberToUpdateViewModel?>> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default);
        public Task<Result> UpdateMemberAsync(int memberId , MemberToUpdateViewModel updateMemberViewModel , CancellationToken ct = default);
        public Task<Result> DeleteMemberAsync(int memberId, CancellationToken ct = default);
        

        
    }
}
