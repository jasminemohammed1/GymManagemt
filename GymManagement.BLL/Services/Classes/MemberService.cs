using GymManagement.BLL.Services.interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GYMProject.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        private readonly IGenericRepository<MemberShips> _membershipsrepo;
        private readonly IGenericRepository<Plan> _planrepo;
        private readonly IGenericRepository<HealthRecord> _HealthRecordrepo;
        private readonly IGenericRepository<Booking> _Bookingrepo;


        public MemberService(IGenericRepository<Member> repo , 
            IGenericRepository<MemberShips> membershipsrepo , IGenericRepository<Plan > plan,
            IGenericRepository<HealthRecord> healthrecordrepo,
            IGenericRepository<Booking> bookingrepo
            )
        {
            _memberRepo = repo;
            _membershipsrepo = membershipsrepo;
            _planrepo = plan;
            _HealthRecordrepo = healthrecordrepo;
            _Bookingrepo = bookingrepo;

           

        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel createMemberViewModel, CancellationToken ct)
        {

            //Email & phone must be unique
            //Check Email exist
            var EmailExists = await _memberRepo.AnyAsync(x => x.Email == createMemberViewModel.Email, ct);
            //Check phone exist 
            var PhoneExists = await _memberRepo.AnyAsync(x => x.Phone == createMemberViewModel.Phone, ct);
            //Email or phone exist => return false
            if (EmailExists || PhoneExists)
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

            var res = await _memberRepo.AddAsync(member, ct);
            return res > 0;

        }

        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member =  await _memberRepo.GetByIdAsync(memberId);
            if (member == null) return false;

            // this checking in booking will cause problem next 

            var booingExists = await _Bookingrepo.AnyAsync(x => x.MemberId == memberId && x.Session.StartDate > DateTime.Now , ct );
            if(booingExists) return false;
            var res = await _memberRepo.DeleteAsync(member, ct);
            return res > 0;

        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberRepo.GetAllAsync(ct: ct);
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

       

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await  _memberRepo.GetByIdAsync(memberId, ct);
            if (member == null) return null;
            var model = new MemberToUpdateViewModel()
            {
                BuildingNumber = member.Address.BuildeingNumber,
                City = member.Address.City,
                Email = member.Email,
                Name = member.Name,
                Phone = member.Phone,
                Photo = member.Photo,
                Street = member.Address.Street

            };
            return model;
        }

        public async Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel updateMemberViewModel, CancellationToken ct = default)
        {
            var member =  await _memberRepo.GetByIdAsync(memberId, ct);
            if(member == null) return false;
            var EmailExists = await  _memberRepo.AnyAsync(x => x.Email == updateMemberViewModel.Email && x.Id != memberId, ct);
            var PhoneExists = await _memberRepo.AnyAsync(x => x.Phone == updateMemberViewModel.Phone && x.Id != memberId, ct);
            if (EmailExists || PhoneExists) return false;
            member.Email = updateMemberViewModel.Email;
            member.Phone = updateMemberViewModel.Phone;
            member.Phone = updateMemberViewModel.Phone;
            member.Address.BuildeingNumber = updateMemberViewModel.BuildingNumber;
            member.Address.Street = updateMemberViewModel.Street;
            member.Address.City = updateMemberViewModel.City;
            member.UpdatedAt = DateTime.Now;

           var res = await  _memberRepo.UpdateAsync(member, ct );
            return res > 0;
            
        }

        public async Task<MemberViewModel?> ViewMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var member = await  _memberRepo.GetByIdAsync(memberId, ct);
            if (member == null) return null;
            var model = new MemberViewModel()
            {
                Name = member.Name,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                Addresss = $"{member.Address.BuildeingNumber} - {member.Address.Street} - {member.Address.City}",
                Phone = member.Phone,
                Email = member.Email,
                DateOfBirth = member.DateOfBirth.ToShortDateString()


            };
            var membership = await  _membershipsrepo.FirstOrDefaultAsync(x => x.MemberId == memberId && x.EndDate > DateTime.Now);
            if(membership is not null)
            {
                model.MemberShipEndDate = membership.EndDate.ToShortDateString();
                model.MemberShipStartDate = membership.CreatedAt.ToShortDateString();
                var plan =  await _planrepo.GetByIdAsync(membership.PlanId, ct);
                model.PlanName = plan?.Name;
            }

            return model;




           
        }

        public async Task<HealthRecordViewModel?> ViewMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var healthrecord = await _HealthRecordrepo.FirstOrDefaultAsync(x => x.HealthRecordMemberId == memberId);
            if (healthrecord == null) return null;
            var model = new HealthRecordViewModel()
            {
               BloodType = healthrecord.BloodType,
               Height = healthrecord.Height,
               Note = healthrecord.Note,
               Weight = healthrecord.Weight,    
            };
            return model;
        }
    }
}

        