using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
namespace HouseRentingSystem.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(12)]
        public string FirstName { get; init; } = null!;
        [Required]
        [MaxLength(15)]
        public string LastName { get; init; } = null!;
    }
}
