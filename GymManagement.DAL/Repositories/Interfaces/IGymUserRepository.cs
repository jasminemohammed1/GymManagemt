using GymManagement.DAL.Models;
using GYMProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    internal interface IGymUserRepository<T> where T: GymUser
    {
        public Task<IEnumerable<T>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        public Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
        public Task<int> UpdateAsync(T user, CancellationToken ct);
        public Task<int> AddAsync(T user, CancellationToken ct);
        public Task<int> DeleteAsync(T user, CancellationToken ct);
    }
}
