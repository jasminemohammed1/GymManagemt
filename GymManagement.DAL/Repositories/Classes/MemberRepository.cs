using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GYMProject.DBContexts;
using GYMProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class MemberRepository : IGymUserRepository<Member>
    {
        private readonly GymDBContext _dbcontext;
        public MemberRepository( GymDBContext db )
        {
            _dbcontext = db;
        }

        public async Task<int> AddAsync(Member user, CancellationToken ct)
        {
            _dbcontext.Members.Add(user);
            return await _dbcontext.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteAsync(Member user, CancellationToken ct)
        {
            _dbcontext.Members.Remove(user);
            return await _dbcontext.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<Member>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<Member> res = tracking ? _dbcontext.Members : _dbcontext.Members.AsNoTracking();
            return await res.ToListAsync(ct);
        }

        public async Task<Member?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbcontext.Members.FindAsync(id, ct);

        }

        public  async Task<int> UpdateAsync(Member user, CancellationToken ct)
        {
            _dbcontext.Members.Update(user);
            return await _dbcontext.SaveChangesAsync(ct);
        }
    }
}
