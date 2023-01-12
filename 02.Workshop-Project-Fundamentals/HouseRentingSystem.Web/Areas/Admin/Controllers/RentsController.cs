using HouseRentingSystem.Services.Rents;
using HouseRentingSystem.Services.Rents.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
    public class RentsController : AdminController
    {
        private readonly IRentService rents;
        private readonly IMemoryCache cache;

        public RentsController(IRentService _rents,
            IMemoryCache _cache)
        {
            this.rents = _rents;
            this.cache = _cache;
        }

        [Route("Rents/All")]
        public IActionResult All()
        {
            var rents = this.cache
                .Get<IEnumerable<RentServiceModel>>(AdminConstants.RentsCacheKey);

            if (rents == null)
            {
                rents = this.rents.All();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                this.cache.Set(AdminConstants.RentsCacheKey, rents, cacheOptions);
            }

            return View(rents);
        }
    }
}
