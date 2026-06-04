using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
   public interface IMemberRepository
    {
        public Task<Member?> GetMemberDetailsAsync(int memberId , CancellationToken ct );
        public Task<Member ?> GetMemberHealthRecord( int memberId , CancellationToken ct );
    }
}
