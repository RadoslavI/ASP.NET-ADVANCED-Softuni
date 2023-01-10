using HouseRentingSystem.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
	public class UsersController : AdminController
	{
		private readonly IUserService users;

		public UsersController(IUserService _users) 
		{
			this.users = _users;
		}

		[Route("Users/All")]
		public IActionResult All() 
		{	
			var users = this.users.All();
			return View(users);
		}
	}
}
