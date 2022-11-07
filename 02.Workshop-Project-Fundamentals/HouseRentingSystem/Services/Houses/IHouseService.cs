using HouseRentingSystem.Services.Models;

namespace HouseRentingSystem.Services.Houses
{
	public interface IHouseService
	{
		IEnumerable<HouseIndexServiceModel> LastThreeHouses();
	}
}
