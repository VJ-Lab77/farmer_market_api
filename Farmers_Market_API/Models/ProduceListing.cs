using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmerMarketAPI.Models;

public class ProduceListing
{
    [Key]
    public int ListingId { get; set; }
    
    [Required]
    public int FarmerId { get; set; }
    
    [Required][MinLength(3)]
    public string ProductName { get; set; } = string.Empty;
    
    [Required]
    public Category Category { get; set; }
    
    [Required][Range(0.01, double.MaxValue)]
    public double PricePerKg { get; set; }
    
    [Required][Range(0.01, double.MaxValue)]
    public double QuantityKg { get; set; }
    
    public bool IsAvailable { get; set; } = true;
    
    [Required]
    public DateTime HarvestDate { get; set; }
    
    public DateTime DateListed { get; set; } = DateTime.Now;
    
    public string? Description { get; set; }
    
    [ForeignKey("FarmerId")]
    public virtual Farmer? Farmer { get; set; }
    
    public string GetFormattedSummary() => $"{ProductName} - {QuantityKg}kg @ R{PricePerKg:F2}/kg = R{CalculateRevenue():F2}";
    public double CalculateRevenue() => QuantityKg * PricePerKg;
    public double CalculateRevenue(double discountPercentage) => CalculateRevenue() * (1 - discountPercentage / 100);
    public string GetCategoryLabel() => Category switch
    {
        Category.Vegetables => "Vegetables",
        Category.Fruit => "Fruit",
        Category.Grain => "Grain",
        Category.Dairy => "Dairy",
        Category.Other => "Other",
        _ => "Unknown"
    };
}