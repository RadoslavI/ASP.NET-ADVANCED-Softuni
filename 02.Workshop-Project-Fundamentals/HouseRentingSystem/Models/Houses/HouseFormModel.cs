using HouseRentingSystem.Services.Houses.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using static HouseRentingSystem.Data.DataConstants;

namespace HouseRentingSystem.Models.Houses
{
	public class HouseFormModel : IHouseModel
	{
        public HouseFormModel()
        {
            Categories = new List<HouseCategoryServiceModel>();
        }

        [Required]
		[StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
		public string Title { get; init; }

		[Required]
		[StringLength (AddressMaxLength, MinimumLength = AddressMinLength)]
		public string Address { get; init; }

		[Required]
		[StringLength(DescruptionMaxLength, MinimumLength = DescruptionMinLength)]
		public string Description{ get; init; }

		[Required]
		[Display(Name = "Image Url")]
		public string ImageUrl { get; init; }

		[Required]
		[Range(0.00, PricePerMonthMax,
			ErrorMessage = "Price per month must be a positive number and less than {2} BGN.")]
		[Display(Name = "Price per Month")]
		public decimal PricePerMonth { get; init; }

		[Display(Name = "Category")]
		public int CategoryId { get; init; }

		public IEnumerable<HouseCategoryServiceModel> Categories { get; set; }
	}
}
