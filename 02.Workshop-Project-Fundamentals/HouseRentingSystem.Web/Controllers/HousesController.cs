﻿using HouseRentingSystem.Web.Infrastructure;
using HouseRentingSystem.Web.Models.Houses;
using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Houses;
using HouseRentingSystem.Services.Houses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using static HouseRentingSystem.Web.Areas.Admin.AdminConstants;
using Microsoft.Extensions.Caching.Memory;
using HouseRentingSystem.Web.Areas.Admin;

namespace HouseRentingSystem.Web.Controllers
{
    public class HousesController : Controller
    {
        private readonly IHouseService houses;
        private readonly IAgentService agents;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;

        public HousesController(IHouseService _houses, 
            IAgentService _agents,
            IMapper _mapper,
            IMemoryCache _cache)
        {
            this.houses = _houses;
            this.agents = _agents;
            this.mapper = _mapper;
            this.cache = _cache;
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
        [HttpGet]
        public IActionResult Mine()
        {
            if (this.User.IsInRole(AdminRoleName))
            {
                return RedirectToAction("Mine", "Houses", new { area = AreaName });
            }

            IEnumerable<HouseServiceModel> myHouses;
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

        public IActionResult Details(int id, string information)
        {
            if (!this.houses.Exists(id))
            {
                return BadRequest();
            }

            var houseModel = this.houses.HouseDetailsById(id);

            if(information != houseModel.GetInformation())
            {
                return BadRequest();
            }

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

            var newHouseId = houses.Create(model.Title, model.Address, 
                model.Description, model.ImageUrl, model.PricePerMonth, 
                model.CategoryId, agentId);

            TempData["message"] = "You have successfully added a house!";

            return RedirectToAction(nameof(Details), new { id = newHouseId, information = model.GetInformation() });
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!this.houses.Exists(id))
            {
                return BadRequest();
            }

            if (!this.houses.HasAgentWithId(id, this.User.Id())
                && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var house = this.houses.HouseDetailsById(id);

            var houseCategoryId = this.houses.GetHouseCategoryId(house.Id);

            var houseModel = this.mapper.Map<HouseFormModel>(house);
            houseModel.CategoryId = houseCategoryId;
            houseModel.Categories = this.houses.AllCategories();

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

            if (!this.houses.HasAgentWithId(id, this.User.Id())
                && !this.User.IsAdmin())
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

            TempData["message"] = "You have successfully edited a house!";

            return RedirectToAction(nameof(Details), new { id = id, information = model.GetInformation() });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!this.houses.Exists(id))
            {
                return BadRequest();
            }

            if (!this.houses.HasAgentWithId(id, this.User.Id())
                && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var house = this.houses.HouseDetailsById(id);

            var model = this.mapper.Map<HouseDetailsViewModel>(house);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(HouseDetailsViewModel model)
        {
            if (!this.houses.Exists(model.Id))
            {
                return BadRequest();
            }

            if (!this.houses.HasAgentWithId(model.Id, this.User.Id())
                && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            this.houses.Delete(model.Id);

            TempData["message"] = "You have successfully deleted a house!";

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Rent(int id)
        {
            if (!this.houses.Exists(id))
            {
                return BadRequest();
            }

            if (this.agents.ExistsById(this.User.Id())
                && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            if (this.houses.IsRented(id))
            {
                return BadRequest();
            }

            this.houses.Rent(id, this.User.Id());

            this.cache.Remove(AdminConstants.RentsCacheKey);

            TempData["message"] = "You have successfully rented a house!";

            return RedirectToAction(nameof(Mine));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Leave(int id)
        {
            if (!this.houses.Exists(id) ||
                !this.houses.IsRented(id))
            {
                return BadRequest();
            }

            if (!this.houses.IsRentedByUserWithId(id, this.User.Id()))
            {
                return Unauthorized();
            }

            this.houses.Leave(id);

            this.cache.Remove(AdminConstants.RentsCacheKey);

            TempData["message"] = "You have successfully left a house!";

            return RedirectToAction(nameof(Mine));
        }
    }
}
