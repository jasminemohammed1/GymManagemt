using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    internal class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _repo;
        public MemberService(IGenericRepository<Member> repo)
        {
            _repo = repo;
            
        }
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
          var members =  await _repo.GetAllAsync(ct: ct);
            if (!members.Any()) return [];
            else
            {
                var memberViewModel = members.Select(m => new MemberViewModel()
                {
                    Email = m.Email,
                    Id = m.Id,
                    Gender = m.Gender.ToString(),
                    Phone = m.Phone,
                    Name = m.Name,
                    

                });

                return memberViewModel;
            }
            
        }
    }
}
