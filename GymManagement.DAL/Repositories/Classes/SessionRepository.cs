using GymManagement.BLL.Services.interfaces;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using GYMProject.DBContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionRepository : GenericRepository<Sessions>, ISessionRepository
    {
        private readonly GymDBContext _db;
        public SessionRepository(GymDBContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Sessions>> GetAllSessionsWithTrainerandCategory(CancellationToken ct)
        {
            var query = _db.Sessions.AsNoTracking().Include(x => x.Trainer).Include(x => x.Category);
            return await query.ToListAsync();
        }

        public async Task<int> GetBookedSlotsAsync(int Id, CancellationToken ct)
        {
            return await _db.Set<Booking>().CountAsync(x => x.Id == Id);
        }
    }
}
