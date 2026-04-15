using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmers_Market_API.Models
{
    public class ProduceListing
    {
        public int ListingId { get; set; }
        public int FarmerId { get; set; }
        public string ProduceName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public double PricePerKg { get; set; } = 0.0;
        public double QuantityKg { get; set; } = 0.0;
        public bool IsAvailable { get; set; } = true;
        public DateTime HarvestDate  { get; set; } = DateTime.Now;
        public DateTime DateListed { get; set; } = DateTime.Now;
        public string? Description { get; set; }

        public ProduceListing(){}

        public string GetFormattedSummary()
        {
            return $"""
            Produce: {ProduceName} 
            Category: {Category}
            Price: ${PricePerKg}/kg
            Quantity: {QuantityKg}kg
            Description: {Description}  
            """;
        }

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