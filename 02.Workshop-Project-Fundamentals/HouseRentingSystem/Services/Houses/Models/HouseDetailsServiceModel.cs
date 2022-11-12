﻿using HouseRentingSystem.Services.Agents;

namespace HouseRentingSystem.Services.Houses.Models
{
    public class HouseDetailsServiceModel : HouseServiceModel
    {
        public string Description { get; init; }

        public string Category { get; init; }

        public AgentServiceModel Agent { get; init; }
    }
}