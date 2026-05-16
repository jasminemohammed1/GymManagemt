using GymManagement.DAL.Repositories.Interfaces;
using GYMProject.DBContexts;
using GYMProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace GymManagement.DAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {
        private readonly  GymDBContext  _dbcontext ;
        public PlanRepository(GymDBContext db)
        {
            _dbcontext = db;    
        }


        public async Task<int> AddAsync(Plan plnan, CancellationToken ct)
        {
            _dbcontext.Plans.Add(plnan);
            return await _dbcontext.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteAsync(Plan plan, CancellationToken ct)
        {
            _dbcontext.Plans.Remove(plan);
            return await _dbcontext.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<Plan> res = tracking ? _dbcontext.Plans : _dbcontext.Plans.AsNoTracking();
            return await res.ToListAsync(ct);
        }

        public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbcontext.Plans.FindAsync(id , ct );
        }

        public async Task<int> UpdateAsync(Plan plan, CancellationToken ct)
        {
            _dbcontext.Plans.Update(plan);
            return await _dbcontext.SaveChangesAsync(ct);
        }
    }
}
