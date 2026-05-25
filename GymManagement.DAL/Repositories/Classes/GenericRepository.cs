using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GYMProject.DBContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDBContext _dbContext;
        private readonly DbSet<TEntity> _set;
        public GenericRepository(GymDBContext db)
        {
            _dbContext = db;
            _set = _dbContext.Set<TEntity>();
            
        }
        public async Task<int> AddAsync(TEntity entity, CancellationToken ct)
        {
           _set.Add(entity);
            return  await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteAsync(TEntity entity, CancellationToken ct)
        {
               _set.Remove(entity);
            return await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
           IQueryable<TEntity> query = tracking ? _set : _set.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
          return await  _set.FindAsync(id, ct);

        }

        public async Task<int> UpdateAsync(TEntity entity, CancellationToken ct)
        {
            _set.Update(entity);
          return  await  _dbContext.SaveChangesAsync(ct);
        }
    }
}
