﻿using HouseRentingSystem.Infrastructure;
using HouseRentingSystem.Models.Houses;
using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Houses;
using HouseRentingSystem.Services.Houses.Models;
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
        public IActionResult All([FromQuery] AllHousesQueryModel query)
        {
            var queryResult = this.houses.All(
                query.Category,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllHousesQueryModel.HousesPerPage);

            query.TotalHousesCount = queryResult.TotalHousesCount;
            query.Houses = queryResult.Houses;

            var houseCategories = this.houses.AllCategoriesNames();
            query.Categories = houseCategories;

            return View(query);
        }

        [Authorize]
        public IActionResult Mine()
        {
            IEnumerable<HouseServiceModel> myHouses = null;

            var userId = this.User.Id();

            if (this.agents.ExistsById(userId))
            {
                var currentAgentId = this.agents.GetAgentId(userId);

                myHouses = this.houses.AllHousesByAgentId(currentAgentId);
            }
            else
            {
                myHouses = this.houses.AllHousesByUserId(userId);
            }

            return View(myHouses);
        }

        public IActionResult Details(int id)
        {
            if (!this.houses.Exists(id))
            {
                return BadRequest();
            }

            var houseModel = this.houses.HouseDetailsById(id);

            return View(houseModel);
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

            if (!houses.CategoryExists(model.CategoryId))
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
            if (!this.houses.Exists(id))
            {
                return BadRequest();
            }

            if (!this.houses.HasAgentWithId(id, this.User.Id()))
            {
                return Unauthorized();
            }

            var house = this.houses.HouseDetailsById(id);

            var houseCategoryId = this.houses.GetHouseCategoryId(house.Id);

            var houseModel = new HouseFormModel()
            {
                Title = house.Title,
                Address = house.Address,
                Description = house.Description,
                ImageUrl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                CategoryId = houseCategoryId,
                Categories = this.houses.AllCategories()
            };

            return View(houseModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, HouseFormModel model)
        {
            if (!this.houses.Exists(id))
            {
                return this.View();
            }

            if (!this.houses.HasAgentWithId(id, this.User.Id()))
            {
                return Unauthorized();
            }

            if (!this.houses.CategoryExists(model.CategoryId))
            {
                this.ModelState.AddModelError(nameof(model.CategoryId),
                    "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = this.houses.AllCategories();

                return View(model);
            }

            this.houses.Edit(id, model.Title, model.Address, model.Description,
                model.ImageUrl, model.PricePerMonth, model.CategoryId);

            return RedirectToAction(nameof(Details), new { id = id });
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
