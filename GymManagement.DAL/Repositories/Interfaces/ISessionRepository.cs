using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.interfaces
{
    public interface ISessionRepository 
    {
        public Task<IEnumerable<Sessions>> GetAllSessionsWithTrainerandCategory(CancellationToken ct);
        public Task<int> GetBookedSlotsAsync(int Id ,CancellationToken ct);
    }
}
