using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HouseRentingSystem.Data.DataConstants;
#nullable disable

namespace HouseRentingSystem.Data.Entities
{
    public class Agent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; init; }

    }

    //        •	Id – a unique integer, Primary Key
    //•	PhoneNumber – a string with min length 7 and max length 15 (required)
    //•	UserId – a string (required)
    //•	User – an IdentityUser object
}
