using HouseRentingSystem.Data;
using HouseRentingSystem.Services.Statistics.Models;
using MessagePack;

namespace HouseRentingSystem.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly HouseRentingDbContext data;

        public StatisticsService(HouseRentingDbContext _data)
        {
            this.data = _data;
        }

        public StatisticsServiceModel Total()
        {
            var totalHouses = data.Houses.Count();
            var totalRents = data.Houses
                .Where(h => h.RenterId != null).Count();

            return new StatisticsServiceModel
            {
                TotalHouses = totalHouses,
                TotalRents = totalRents
            };
        }


    }
}
