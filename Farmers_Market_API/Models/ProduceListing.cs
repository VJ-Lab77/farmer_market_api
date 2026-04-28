using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Farmers_Market_API.Enums;

namespace Farmers_Market_API.Models
{
    public class ProduceListing
    {
        internal readonly int Id;

        public int ListingId { get; set; }
        public int FarmerId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public Category Category { get; set; }

        public double PricePerKg { get; set; }
        public double QuantityKg { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime HarvestDate { get; set; } = DateTime.Now;
        public DateTime DateListed { get; set; } = DateTime.Now;

        public string? Description { get; set; }

        public ProduceListing() { }

        public string GetFormattedSummary()
        {
            return $"""
            Produce: {ProductName}
            Price: R{PricePerKg:F2}
            Quantity: {QuantityKg} kg
            Category: {Category}
            Description: {Description ?? "None Provided"}
            """;
        }

        public double CalculateRevenue()
        {
            return PricePerKg * QuantityKg;
        }
    }
}