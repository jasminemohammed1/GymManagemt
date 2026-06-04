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
        public Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        public Task<bool> CreateMemberAsync(CreateMemberViewModel createMemberViewModel , CancellationToken ct = default);
        public Task<MemberViewModel?> ViewMemberDetailsAsync(int memberId, CancellationToken ct = default);
        public Task<HealthRecordViewModel ?> ViewMemberHealthRecordAsync(int memberId, CancellationToken ct = default);
        
        public Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default);
        public Task<bool> UpdateMemberAsync(int memberId , MemberToUpdateViewModel updateMemberViewModel , CancellationToken ct = default);
        public Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default);
        

        
    }
}
