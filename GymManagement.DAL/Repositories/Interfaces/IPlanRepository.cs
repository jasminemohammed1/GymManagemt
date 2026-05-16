using GYMProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        public Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false , CancellationToken ct = default);
        public Task<Plan?> GetByIdAsync(int id , CancellationToken ct = default);
        public Task<int> UpdateAsync(Plan plan , CancellationToken ct );
        public Task<int> AddAsync (Plan plnan  , CancellationToken ct );
        public Task<int> DeleteAsync(Plan plan , CancellationToken ct );
    }
}
