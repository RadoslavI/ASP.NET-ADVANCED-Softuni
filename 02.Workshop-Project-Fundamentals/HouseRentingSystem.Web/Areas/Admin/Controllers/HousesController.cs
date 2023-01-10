using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Houses;
using HouseRentingSystem.Web.Areas.Admin.Models;
using HouseRentingSystem.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
    public class HousesController : Controller
    {
        private readonly IHouseService houses;
        private readonly IAgentService agents;

        public HousesController(IHouseService _houses, IAgentService _agents)
        {
            this.houses = _houses;
            this.agents = _agents;
        }

        [Route("Houses/Mine")]
        [HttpGet]
        public IActionResult Mine()
        {
            var myHouses = new MyHousesViewModel();

            var adminUserId = this.User.Id();
            myHouses.RentedHouses = this.houses.AllHousesByUserId(adminUserId);

            var adminAgentId = this.agents.GetAgentId(adminUserId);
            myHouses.AddedHouses = this.houses.AllHousesByAgentId(adminAgentId);

            return View(myHouses);
        }
    }
}
