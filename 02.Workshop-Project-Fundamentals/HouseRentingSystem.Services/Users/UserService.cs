using AutoMapper;
using AutoMapper.QueryableExtensions;
using HouseRentingSystem.Services.Data;
using HouseRentingSystem.Services.Users.Models;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace HouseRentingSystem.Services.Users
{
    public class UserService : IUserService
    {
        private readonly HouseRentingDbContext data;
        private readonly IMapper mapper;
        public UserService(HouseRentingDbContext _data,
            IMapper _mapper)
        {
            this.data = _data;
            this.mapper = _mapper;
		}

		public IEnumerable<UserServiceModel> All()
		{
			var allUsers = new List<UserServiceModel>();

            var agents = this.data
                .Agents
                .Include(a => a.User)
                .ProjectTo<UserServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

            allUsers.AddRange(agents);

            var users = this.data
                .Users
                .Where(u => !this.data.Agents.Any(a => a.UserId == u.Id))
                .ProjectTo<UserServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

            allUsers.AddRange(users);

            return allUsers;
		}

		public string UserFullName(string userId)
        {
            var user = this.data.Users.Find(userId);

            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName))
            {
                return null;
            }

            return user.FirstName + " " + user.LastName;
        }
        public bool UserHasRents(string userId)
            => this.data.Houses.Any(h => h.RenterId == userId);
    }
}
