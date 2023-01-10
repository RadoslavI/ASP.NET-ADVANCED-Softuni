using HouseRentingSystem.Services.Rents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Services.Rents
{
    public interface IRentService
    {
        IEnumerable<RentServiceModel> All(); 
    }
}
