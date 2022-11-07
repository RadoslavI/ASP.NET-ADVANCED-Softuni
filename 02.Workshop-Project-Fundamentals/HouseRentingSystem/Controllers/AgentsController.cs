﻿using HouseRentingSystem.Infrastructure;
using HouseRentingSystem.Models.Agents;
using HouseRentingSystem.Services.Agents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HouseRentingSystem.Controllers
{
	public class AgentsController : Controller
	{
		private readonly IAgentService agents;

		public AgentsController(IAgentService _agents)
		{
			this.agents = _agents;
		}

		[Authorize]
		public IActionResult Become()
		{
			if (agents.ExistsById(User.Id()))
			{
				return BadRequest();
			}

			return View();
		}

        [Authorize]
		[HttpPost]
        public IActionResult Become(BecomeAgentFormModel model)
        {
			var userId = this.User.Id();

            if (agents.ExistsById(User.Id()))
            {
                return BadRequest();
            }

            if (agents.UserWithPhoneNumberExists(model.PhoneNumber))
            {
                ModelState.AddModelError(nameof(model.PhoneNumber),
					"Phone number already exists. Enter another one.");
            }

            if (agents.UserHasRents(userId))
            {
                ModelState.AddModelError("Error",
                    "You should have no rents to become an agent!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.agents.Create(userId, model.PhoneNumber);

            return RedirectToAction(nameof(HousesController.All), "Houses");
        }
    }
}
