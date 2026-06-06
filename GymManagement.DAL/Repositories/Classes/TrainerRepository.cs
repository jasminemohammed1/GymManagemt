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
    public class TrainerRepository : GenericRepository<Trainer>, ITrainerRepository
    {
        private readonly GymDBContext db;

        public TrainerRepository(GymDBContext db) : base(db)
        {
            this.db = db;
        }
      
        public async Task<int> GetTrainerCount(CancellationToken ct)
        {
            return await  db.Trainers.CountAsync();
        }
    }
}
