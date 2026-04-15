using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmers_Market_API.Models
{
    public class ProduceListing
    {
        private int ListingId { get; set; }
        private int FarmerId { get; set; }
        private string ProduceName { get; set; } = string.Empty;
        private string Category { get; set; } = string.Empty;
        private double PricePerKg { get; set; } = 0.0;
        private double QuantityKg { get; set; } = 0.0;
        private bool IsAvailable { get; set; } = true;
        private DateTime HarvestDate  { get; set; } = DateTime.Now;
        private DateTime DateListed { get; set; } = DateTime.Now;
        private string? Description { get; set; }

        public ProduceListing(){}

        public ProduceListing(int listingId, int farmerId, string produceName, string category, double pricePerKg, double quantityKg, bool isAvailable, DateTime harvestDate, DateTime dateListed, string? description)
        {
            ListingId = listingId;
            FarmerId = farmerId;
            ProduceName = produceName;
            Category = category;
            PricePerKg = pricePerKg;
            QuantityKg = quantityKg;
            IsAvailable = isAvailable;
            HarvestDate = harvestDate;
            DateListed = dateListed;
            Description = description;
        }
    }
}