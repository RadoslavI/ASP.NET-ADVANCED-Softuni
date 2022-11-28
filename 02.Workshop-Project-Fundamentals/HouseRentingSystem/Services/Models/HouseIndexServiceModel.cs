#nullable disable
using HouseRentingSystem.Services.Houses.Models;

namespace HouseRentingSystem.Services.Models
{
	public class HouseIndexServiceModel : IHouseModel
	{
		public int Id { get; init; }

		public string Title { get; init; }

		public string ImageUrl { get; init; }

		public string Address { get; init; }
	}
}
