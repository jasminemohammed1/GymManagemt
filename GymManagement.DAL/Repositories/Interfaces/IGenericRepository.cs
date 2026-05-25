using GymManagement.DAL.Models;
using GYMProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
   public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {

        public Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        public Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        public Task<int> UpdateAsync(TEntity entity, CancellationToken ct);
        public Task<int> AddAsync(TEntity entity, CancellationToken ct);
        public Task<int> DeleteAsync(TEntity entity, CancellationToken ct);

    }
}
