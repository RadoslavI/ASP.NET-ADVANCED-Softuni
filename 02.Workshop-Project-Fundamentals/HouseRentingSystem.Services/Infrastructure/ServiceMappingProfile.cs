using AutoMapper;
using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Data.Entities;
using HouseRentingSystem.Services.Houses.Models;
using HouseRentingSystem.Services.Models;
using HouseRentingSystem.Services.Users.Models;

namespace HouseRentingSystem.Services.Infrastructure
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            this.CreateMap<House, HouseServiceModel>()
                .ForMember(h => h.IsRented, 
                cfg => cfg.MapFrom(h => h.RenterId != null));

            this.CreateMap<House, HouseDetailsServiceModel>()
                .ForMember(h => h.IsRented,
                cfg => cfg.MapFrom(h => h.RenterId != null))
                .ForMember(h => h.Category,
                cfg => cfg.MapFrom(h => h.Category.Name));

            this.CreateMap<Agent, AgentServiceModel>()
                .ForMember(ag => ag.Email,
                cfg => cfg.MapFrom(ag => ag.User.Email));

            this.CreateMap<House, HouseIndexServiceModel>();

            this.CreateMap<Category, HouseCategoryServiceModel>();

            this.CreateMap<Agent, UserServiceModel>()
                .ForMember(us => us.Email, cfg => cfg.MapFrom(ag => ag.User.Email))
                .ForMember(us => us.FullName, cfg => cfg
                .MapFrom(ag => ag.User.FirstName + " " + ag.User.LastName));

			this.CreateMap<User, UserServiceModel>()
				.ForMember(us => us.PhoneNumber, cfg => cfg.MapFrom(us => string.Empty))
				.ForMember(us => us.FullName, cfg => cfg
				    .MapFrom(us => us.FirstName + " " + us.LastName));
		}
    }
}
