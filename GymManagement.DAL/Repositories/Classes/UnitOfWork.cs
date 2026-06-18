using GymManagement.BLL.Services.interfaces;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GYMProject.DBContexts;

namespace GymManagement.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDBContext _dbcontext;
        private readonly Dictionary<string, object> repositories = [];

        public UnitOfWork(GymDBContext db, ISessionRepository repo , IMemberRepository memberRepository)
        {
            _dbcontext = db;
            SessionRepository = repo;
            MemberRepository = memberRepository;
            
        }

        public ISessionRepository SessionRepository { get; }  
        public IMemberRepository MemberRepository { get; }
      

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var type = typeof(TEntity).Name;
            if (repositories.TryGetValue(type, out object? repository))
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
            return await _dbcontext.SaveChangesAsync(ct);
        }
    }
}