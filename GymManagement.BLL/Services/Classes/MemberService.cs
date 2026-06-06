using AutoMapper;
using GymManagement.BLL.Common;
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

        private readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork , IMapper mapper) 
            
        {
            
            _unitofwork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<Result> CreateMemberAsync(CreateMemberViewModel createMemberViewModel, CancellationToken ct)
        {

            //Email & phone must be unique
            //Check Email exist
            var EmailExists = await _unitofwork.GetRepository<Member>().AnyAsync(x => x.Email == createMemberViewModel.Email, ct);
            //Check phone exist 
            var PhoneExists = await _unitofwork.GetRepository<Member>().AnyAsync(x => x.Phone == createMemberViewModel.Phone, ct);
            //Email or phone exist => return false
            if (EmailExists || PhoneExists)
            {
                return Result.Validation("Phone or Email Exists before");
            }
            // else create member

            var member = _mapper.Map<Member>(createMemberViewModel);

           _unitofwork.GetRepository<Member>().Add(member);
           var res = await  _unitofwork.SaveChangesAsync();
            return res > 0 ? Result.Ok() : Result.Fail("Cannot Create Member");

        }

        public async Task<Result> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member =  await _unitofwork.GetRepository<Member>().GetByIdAsync(memberId);
            if (member == null) return Result.NotFound("This Member not Found");

            // this checking in booking will cause problem next 

            var booingExists = await _unitofwork.GetRepository<Booking>().AnyAsync(x => x.MemberId == memberId && x.Session.StartDate > DateTime.Now , ct );
            if(booingExists) return Result.Validation("Cannot Delete Member with Existing Booking");
             _unitofwork.GetRepository<Member>().Delete(member);
           var res =  await  _unitofwork.SaveChangesAsync(ct);
            return res > 0 ? Result.Ok() : Result.Fail("Fail To Delete Member");

        }

      

        public async Task<Result<IEnumerable<MemberViewModel>>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _unitofwork.GetRepository<Member>().GetAllAsync(ct: ct);
            if (!members.Any()) return Result<IEnumerable<MemberViewModel>>.Ok([]);
            else
            {
                var memberViewModel = _mapper.Map<IEnumerable<Member>,IEnumerable<MemberViewModel>>(members);

                return Result<IEnumerable<MemberViewModel>>.Ok(memberViewModel);
            }

        }

       

        public async Task<Result<MemberToUpdateViewModel>?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitofwork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member == null) return null;
            var model = _mapper.Map<Member, MemberToUpdateViewModel>(member);
            return Result<MemberToUpdateViewModel>.Ok(model);
        }

       

        public async Task<Result> UpdateMemberAsync(int memberId, MemberToUpdateViewModel updateMemberViewModel, CancellationToken ct = default)
        {
            var member =  await _unitofwork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member == null) return Result.NotFound("Member Not Found");
            var EmailExists = await _unitofwork.GetRepository<Member>().AnyAsync(x => x.Email == updateMemberViewModel.Email && x.Id != memberId, ct);
            var PhoneExists = await _unitofwork.GetRepository<Member>().AnyAsync(x => x.Phone == updateMemberViewModel.Phone && x.Id != memberId, ct);
            if (EmailExists || PhoneExists) return Result.Validation("Email or Phone Exists Before");
            _mapper.Map(updateMemberViewModel, member);
            member.UpdatedAt = DateTime.Now;

           _unitofwork.GetRepository<Member>().Update(member);
            var res =  await _unitofwork.SaveChangesAsync();
            return res > 0 ? Result.Ok() : Result.Fail("Fail To Update Member");
            
        }

        public async Task<Result<MemberViewModel>?> ViewMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var member = await  _unitofwork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member == null) return null;

            
            var model = _mapper.Map<Member, MemberViewModel>(member);
            var membership = await _unitofwork.GetRepository<MemberShips>().FirstOrDefaultAsync(x => x.MemberId == memberId && x.EndDate > DateTime.Now);
            if(membership is not null)
            {
                model.MemberShipEndDate = membership.EndDate.ToShortDateString();
                model.MemberShipStartDate = membership.CreatedAt.ToShortDateString();
                var plan =  await _unitofwork.GetRepository<Plan>().GetByIdAsync(membership.PlanId, ct);
                model.PlanName = plan?.Name;
            }

            return Result<MemberViewModel>.Ok( model);




           
        }

        public async Task<Result<HealthRecordViewModel>?> ViewMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var healthrecord = await _unitofwork.GetRepository<HealthRecord>().FirstOrDefaultAsync(x => x.HealthRecordMemberId == memberId);
            if (healthrecord == null) return null;
            var model = _mapper.Map<HealthRecord, HealthRecordViewModel>(healthrecord);
            return    Result<HealthRecordViewModel>.Ok( model);
        }
    }
}

        