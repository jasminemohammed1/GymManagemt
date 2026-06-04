using GymManagement.DAL.Models;
using GYMProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
   public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {

        public Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        public Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        public  void Update(TEntity entity, CancellationToken ct);
        public void  Add(TEntity entity, CancellationToken ct);
        public void  Delete(TEntity entity, CancellationToken ct);
        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
        public Task<TEntity ?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, CancellationToken ct = default);

    }
}
