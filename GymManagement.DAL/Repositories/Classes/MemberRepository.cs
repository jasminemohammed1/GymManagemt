using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GYMProject.DBContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class MemberRepository: GenericRepository<Member>, IMemberRepository
    {
        private readonly GymDBContext gymDBContext;
        public MemberRepository(GymDBContext db) : base (db)
        {
            gymDBContext = db;
        }
        public async Task<Member?> GetMemberDetailsAsync(int memberId, CancellationToken ct)
        {
            var memberDetail = await gymDBContext.Members.Include(x => x.MemberShips).ThenInclude(x => x.Plan)
                          .FirstOrDefaultAsync(x => x.Id == memberId  , ct);
            return memberDetail;
        }

        public async Task<Member ?> GetMemberHealthRecord(int memberId, CancellationToken ct)
        {
            var res = await gymDBContext.Members.Include(x => x.HealthRecord)
                                .FirstOrDefaultAsync(x => x.Id == memberId , ct);

            return res;


        }
    }
}
