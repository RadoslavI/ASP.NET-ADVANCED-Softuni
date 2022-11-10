using HouseRentingSystem.Models;
using HouseRentingSystem.Services.Houses.Models;
using HouseRentingSystem.Services.Models;

namespace HouseRentingSystem.Services.Houses
{
	public interface IHouseService
	{
		IEnumerable<HouseIndexServiceModel> LastThreeHouses();
		IEnumerable<HouseCategoryServiceModel> AllCategories();
		bool CategoryExists(int categoryId);
		int Create(string title, string address,
			string description, string imageUrl, decimal price,
			int categoryId, int agentId);

		HouseQueryServiceModel All(
			string category = null,
			string searchTerm = null,
			HouseSorting sorting = HouseSorting.Newest,
			int currentPage = 1,
			int housesPerPage = 1);

		IEnumerable<string> AllCategoriesNames();

    }
}
