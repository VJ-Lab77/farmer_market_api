using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Farmers_Market_API.Models
{
    public class Farmer
    {
        static int _idCounter = 1;
        private  int FarmerId { get; set; } = _idCounter++;
        private  string FullName { get; set; } = string.Empty;
        private string Email { get; set; } = string.Empty;
        private string PhoneNumber { get; set; } = string.Empty;
        private string Location { get; set; } = string.Empty;
        private string Province { get; set; } = string.Empty;
        private double Rating { get; set; } = 0.0;
        private bool IsVerified { get; set; } = false;
        
        public Farmer(string fullName, string email, string phoneNumber, string location, string province, double rating, bool isVerified)
        {
            
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            Location = location;
            Province = province;
            Rating = rating;
            this.IsVerified = isVerified;
        }
        
        public Farmer(){}
    }
}