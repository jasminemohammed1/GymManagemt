using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GYMProject.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDBContext _dbcontext;
        private readonly Dictionary<string, object> repositories = [];
        public UnitOfWork(GymDBContext db)
        {
            _dbcontext = db;
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var type = typeof(TEntity).Name;
            if(repositories.TryGetValue(type, out object ? repository))
            {
                return (IGenericRepository<TEntity>)repository;
            }
            else
            {
                var repo = new GenericRepository<TEntity>(_dbcontext);
                repositories[type] = repo;
                return repo;
            }

        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
          return await _dbcontext.SaveChangesAsync( ct );
        }
    }
}
