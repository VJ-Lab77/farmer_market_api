using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Farmers_Market_API.Models
{
    
    public class Farmer
    {
        private static int _idCounter = 1;
        private int FarmerId { get; set; } = _idCounter++;

        // Make these public so the controller can update them
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public double Rating { get; set; } = 0.0;
        public bool IsVerified { get; set; } = false;

        public Farmer(string fullName, string email, string phoneNumber, string location, string province, double rating, bool isVerified)
        {
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            Location = location;
            Province = province;
            Rating = rating;
            IsVerified = isVerified;
        }

        public Farmer() { }

        // Expose ID via method
        public int GetFarmerId()
        {
            return FarmerId;
        }
    }
}