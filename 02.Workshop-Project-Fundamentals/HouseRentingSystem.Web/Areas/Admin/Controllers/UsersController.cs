﻿using HouseRentingSystem.Services.Users;
using HouseRentingSystem.Services.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static HouseRentingSystem.Web.Areas.Admin.AdminConstants;

namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
	public class UsersController : AdminController
	{
		private readonly IUserService users;
		private readonly IMemoryCache cache;

        public UsersController(IUserService _users,
			IMemoryCache _cache) 
		{
			this.users = _users;
			this.cache = _cache;
        }

		[Route("Users/All")]
		public IActionResult All() 
		{
			var users = this.cache
				.Get<IEnumerable<UserServiceModel>>(UsersCacheKey);

			if (users == null) 
			{
				users = this.users.All();

				var cacheOptions = new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

				this.cache.Set(UsersCacheKey, users, cacheOptions);
			}

			return View(users);
		}
	}
}
