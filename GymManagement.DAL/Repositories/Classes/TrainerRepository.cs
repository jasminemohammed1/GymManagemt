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
    internal class TrainerRepository : IGymUserRepository<Trainer>

    {
        private readonly GymDBContext _dbcontext;
        public TrainerRepository(GymDBContext gymDB)
        {
            _dbcontext = gymDB;
        }
        public async Task<int> AddAsync(Trainer user, CancellationToken ct)
        {
            _dbcontext.Trainers.Add(user);
            return await _dbcontext.SaveChangesAsync(ct);

        }

        public async Task<int> DeleteAsync(Trainer user, CancellationToken ct)
        {
            _dbcontext.Trainers.Remove(user);
            return await _dbcontext.SaveChangesAsync(ct);

        }

        public async Task<IEnumerable<Trainer>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<Trainer> res = tracking ? _dbcontext.Trainers : _dbcontext.Trainers.AsNoTracking();
            return await res.ToListAsync(ct);

        }

        public async Task<Trainer?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbcontext.Trainers.FindAsync(id, ct);
        }

        public async Task<int> UpdateAsync(Trainer user, CancellationToken ct)
        {
            _dbcontext.Trainers.Update(user);
            return await _dbcontext.SaveChangesAsync(ct);
        }
    }

}
