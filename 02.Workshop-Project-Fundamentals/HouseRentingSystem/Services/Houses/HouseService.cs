﻿using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Entities;
using HouseRentingSystem.Models;
using HouseRentingSystem.Services.Houses.Models;
using HouseRentingSystem.Services.Models;
using Microsoft.Build.Evaluation;

namespace HouseRentingSystem.Services.Houses
{
	public class HouseService : IHouseService
	{
		private readonly HouseRentingDbContext data;

		public HouseService(HouseRentingDbContext _data)
		{
			this.data = _data;
		}

		public HouseQueryServiceModel All(string category = null, 
			string searchTerm = null, 
			HouseSorting sorting = HouseSorting.Newest, 
			int currentPage = 1, 
			int housesPerPage = 1)
		{
			var housesQuery = this.data.Houses.AsQueryable();

			if (!string.IsNullOrWhiteSpace(category))
			{
				housesQuery = this.data.Houses
					.Where(h => h.Category.Name == category);
			}

			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				housesQuery = housesQuery.Where(h =>
					h.Title.ToLower().Contains(searchTerm.ToLower()) ||
					h.Address.ToLower().Contains(searchTerm.ToLower()) ||
					h.Description.ToLower().Contains(searchTerm.ToLower()));
			}

			housesQuery = sorting switch
			{
				HouseSorting.Price => housesQuery
					.OrderBy(h => h.PricePerMonth),
				HouseSorting.NotRentedFirst => housesQuery
					.OrderBy(h => h.RenterId != null)
					.ThenByDescending(h => h.Id),
					_ => housesQuery.OrderByDescending(h => h.Id)
			};

			var houses = housesQuery
				.Skip((currentPage - 1) * housesPerPage)
				.Take(housesPerPage)
				.Select(h => new HouseServiceModel
				{
					Id = h.Id,
					Title = h.Title,
					Address = h.Address,
					ImageUrl = h.ImageUrl,
					IsRented = h.RenterId != null,
					PricePerMonth = h.PricePerMonth
				})
				.ToList();

			var totalHouses = housesQuery.Count();

			return new HouseQueryServiceModel()
			{
				TotalHousesCount = totalHouses,
				Houses = houses
			};
		}

		public IEnumerable<HouseCategoryServiceModel> AllCategories()
		{
			return this.data
					.Categories
					.Select(c => new HouseCategoryServiceModel
					{
						Id = c.Id,
						Name = c.Name
					})
					.ToList();
		}

		public IEnumerable<string> AllCategoriesNames()
		{
			return this.data
				.Categories
					.Select(c => c.Name)
					.Distinct()
					.ToList();
		}

		public IEnumerable<HouseServiceModel> AllHousesByAgentId(int agentId)
		{
			var houses = this.data
					.Houses
					.Where(h => h.AgentId == agentId)
					.ToList();

			return ProjectToModel(houses);
		}

		public IEnumerable<HouseServiceModel> AllHousesByUserId(string userId)
		{
			var houses = this.data
				.Houses
				.Where(h => h.RenterId == userId)
				.ToList();

			return ProjectToModel(houses);
		}

        private List<HouseServiceModel> ProjectToModel(List<House> houses)
        {
			var resultHouses = houses
				.Select(h => new HouseServiceModel()
				{
					Id = h.Id,
					Title = h.Title,
					Address = h.Address,
					ImageUrl = h.ImageUrl,
					PricePerMonth = h.PricePerMonth,
					IsRented = h.RenterId != null
				})
				.ToList();

			return resultHouses;
        }

        public bool CategoryExists(int categoryId)
			=> this.data.Categories.Any(c => c.Id == categoryId);

		public int Create(string title, string address, 
			string description, string imageUrl, 
			decimal price, int categoryId, int agentId)
		{
			var house = new House
			{
				Title = title,
				Address = address,
				Description = description,
				ImageUrl = imageUrl,
				PricePerMonth = price,
				CategoryId = categoryId,
				AgentId = agentId
			};

			this.data.Houses.Add(house);
			this.data.SaveChanges();

			return house.Id;
		}

		public IEnumerable<HouseIndexServiceModel> LastThreeHouses()
		{
			return this.data
					.Houses
					.OrderByDescending(c => c.Id)
					.Select(c => new HouseIndexServiceModel
					{
						Id = c.Id,
						Title = c.Title,
						ImageUrl = c.ImageUrl
					})
					.Take(3);
		}
	}
}
