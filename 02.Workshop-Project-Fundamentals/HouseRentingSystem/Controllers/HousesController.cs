using HouseRentingSystem.Infrastructure;
using HouseRentingSystem.Models.Houses;
using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Houses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
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

        public IActionResult All()
        {
            return View(new AllHousesQueryModel());
        }

        [Authorize]
        public IActionResult Mine()
        {
            return View(new AllHousesQueryModel());
        }

        public IActionResult Details(int id)
        {
            return View();
        }

        public IActionResult Add()
        {
            if (!this.agents.ExistsById(this.User.Id()))
            {
                return RedirectToAction(nameof(AgentsController.Become), "Agents");
            }

            return View(new HouseFormModel
            {
                Categories = this.houses.AllCategories()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(HouseFormModel model)
        {
            if (!agents.ExistsById(User.Id()))
            {
                return RedirectToAction(nameof(AgentsController.Become), "Agents");
            }

            if (houses.CategoryExists(model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId),
                    "Category doesn't exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = houses.AllCategories();

                return View(model);
            }

            int agentId = agents.GetAgentId(User.Id());

            var newHouseId = houses.Create(model.Title, model.Address, model.Description,
                model.ImageUrl, model.PricePerMonth, model.CategoryId, agentId);

            return RedirectToAction(nameof(Details), new { id = newHouseId });
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            return View(new HouseFormModel());
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, HouseFormModel model)
        {
            return RedirectToAction(nameof(Details), new { id = "1" });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            return View(new HouseDetailsViewModel());
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(HouseDetailsViewModel model)
        {
            return RedirectToAction(nameof(All));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Rent(int id)
        {
            return RedirectToAction(nameof(Mine));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Leave(int id)
        {
            return RedirectToAction(nameof(Mine));
        }
    }
}
