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
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepo;
        public MemberService(IGenericRepository<Member> repo)
        {
            _memberRepo = repo;
            
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel createMemberViewModel, CancellationToken ct)
        {
            
            //Email & phone must be unique
            //Check Email exist
          var EmailExists = await _memberRepo.AnyAsync(x => x.Email==createMemberViewModel.Email, ct);
            //Check phone exist 
            var PhoneExists = await _memberRepo.AnyAsync(x => x.Phone ==createMemberViewModel.Phone, ct);
            //Email or phone exist => return false
            if(EmailExists || PhoneExists)
            {
                return false;
            }
            // else create member

            Member member = new Member()
            {
                Name = createMemberViewModel.Name,
                Email = createMemberViewModel.Email,
                Phone = createMemberViewModel.Phone,
                DateOfBirth = createMemberViewModel.DateOfBirth,
                Address = new Address()
                {
                    BuildeingNumber = createMemberViewModel.BuildingNumber,
                    City = createMemberViewModel.City,
                    Street = createMemberViewModel.Street,
                },
                Gender = createMemberViewModel.Gender,
                
                HealthRecord = new HealthRecord()
                {
                    Weight = createMemberViewModel.HealthRecordViewModel.Weight,
                    Note = createMemberViewModel.HealthRecordViewModel.Note,
                    Height = createMemberViewModel.HealthRecordViewModel.Height,    
                    BloodType = createMemberViewModel.HealthRecordViewModel.BloodType
                },

              
            };

            var res = await _memberRepo.AddAsync(member , ct);
            return res > 0;

        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
          var members =  await _memberRepo.GetAllAsync(ct: ct);
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
