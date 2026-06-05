using AutoMapper;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.PlansViewModel;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Models;
using GYMProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMemberProfiles();
            CreateTrainerProfiles();
            CreatePlanProfiles();
            CreateSessionProfile();
           








        }

        private void CreateMemberProfiles()
        {
            CreateMap<Member, MemberViewModel>()
               .ForMember(des => des.Gender, opts => opts.MapFrom(src => src.Gender))
               .ForMember(des => des.Addresss, opts => opts.MapFrom(src => $"{src.Address.BuildeingNumber} - {src.Address.Street} - {src.Address.City}"))
               .ForMember(des => des.DateOfBirth, opts => opts.MapFrom(src => src.DateOfBirth));
            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();
            CreateMap<Member, MemberToUpdateViewModel>()
                   .ForMember(des => des.Street, opts => opts.MapFrom(src => src.Address.Street))
                   .ForMember(des => des.BuildingNumber, opts => opts.MapFrom(src => src.Address.BuildeingNumber))
                   .ForMember(des => des.Street, opts => opts.MapFrom(src => src.Address.Street));

            CreateMap<MemberToUpdateViewModel, Member>()
                    .ForMember(des => des.Name, opts => opts.Ignore())
                    .ForMember(des => des.Photo, opts => opts.Ignore())
                    //.ForMember(des => des.Address, opts => opts.MapFrom(src => src ))
                    .AfterMap((src, des) =>
                    {
                        des.Address.Street = src.Street;
                        des.Address.BuildeingNumber = src.BuildingNumber;
                        des.Address.City = src.City;
                    });

            CreateMap<CreateMemberViewModel, Member>()
                 .ForMember(des => des.Address, opts => opts.MapFrom(src => new Address()
                 {
                     Street = src.Street,
                     City = src.City,
                     BuildeingNumber = src.BuildingNumber
                 })).ForMember(des => des.HealthRecord, opts => opts.MapFrom(src => src.HealthRecordViewModel));

        }
        private void CreateTrainerProfiles()
        {
            CreateMap<TranierToCreateViewModel, Trainer>()
               .ForMember(des => des.Address, opts => opts.MapFrom(src => new Address()
               {
                   Street = src.Street,
                   City = src.City,
                   BuildeingNumber = src.BuildingNumber

               }));

            CreateMap<Trainer, TrainerDetailsViewModel>()
                .ForMember(des => des.Address, opts => opts.MapFrom(src => $"{src.Address.BuildeingNumber} - {src.Address.Street} - {src.Address.City}"))
                .ForMember(des => des.DateOfBirth, opts => opts.MapFrom(src => src.DateOfBirth.ToShortDateString()));


            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(des => des.Specialization, opts => opts.MapFrom(src => src.speciality.ToString()))

                ;
            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.Street, opts => opts.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.BuildingNumber, opts => opts.MapFrom(src => src.Address.BuildeingNumber));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
                .AfterMap((src, des) =>
                { 
                    des.Address.Street = src.Street;    
                    des.Address.City = src.City;
                    des.Address.BuildeingNumber = src.BuildingNumber;
                })
                .ForMember(dest => dest.Name , opts => opts.Ignore());
        }
        private void CreatePlanProfiles()
        {
            CreateMap<Plan, PlanViewModel>()
                .ForMember(des => des.Duration, opts => opts.MapFrom(src => src.DurationDays ));
            CreateMap<Plan, PlanToUpdateViewModel>();

            CreateMap<PlanToUpdateViewModel, Plan>()
                .ForMember(des => des.Name, opts => opts.Ignore());
               
                
        }
        private void CreateSessionProfile()
        {
            CreateMap<CreateSessionViewModel, Sessions>();
        }
    }
}
