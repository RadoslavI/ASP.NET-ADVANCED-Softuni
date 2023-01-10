using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace HouseRentingSystem.Services.Rents.Models
{
    public class RentServiceModel
    {
        public string HouseTitle { get; init; }
        public string HouseImageURL { get; init; }
        public string AgentFullName { get; init; }
        public string AgentEmail { get; init; }
        public string RenterFullName { get; init; }
        public string RenterEmail { get; init; }
    }
}
