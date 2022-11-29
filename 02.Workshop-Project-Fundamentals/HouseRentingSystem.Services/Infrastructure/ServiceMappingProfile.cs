using AutoMapper;
using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Data.Entities;
using HouseRentingSystem.Services.Houses.Models;
using HouseRentingSystem.Services.Models;

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
        }
    }
}
