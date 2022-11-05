using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Data.DataConstants;
#nullable disable

namespace HouseRentingSystem.Data.Entities
{
    public class Category
    {
        public Category()
        {
            Houses = new List<House>();
        }

        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public List<House> Houses { get; init; }
    }
}
