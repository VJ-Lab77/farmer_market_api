using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Farmers_Market_API.Enums;

namespace Farmers_Market_API.Models
{
    public class Farmer
    {
        public int FarmerId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public Province Province { get; set; }

        public double Rating { get; set; } = 0.0;

        public bool IsVerified { get; set; } = false;

        public Farmer() { }
    }

    
}