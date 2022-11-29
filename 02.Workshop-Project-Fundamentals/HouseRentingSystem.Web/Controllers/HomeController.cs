﻿using HouseRentingSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HouseRentingSystem.Web.Models.Home;
using HouseRentingSystem.Services.Houses;

namespace HouseRentingSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHouseService houses;
        public HomeController(IHouseService _houses)
        {
            this.houses = _houses;
        }

        public IActionResult Index()
        {
            var houses = this.houses.LastThreeHouses();
            return View(houses);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 400)
            {
                return View("Error400");
            }

            if (statusCode == 401)
            {
                return View("Error401");
            }

            return View();
        }
    }
}