using System.ComponentModel.DataAnnotations;

namespace FarmerMarketAPI.Models;

public class CreateOrderRequest
{
    [Required]
    public int BuyerId { get; set; }

    [Required]
    public int ListingId { get; set; }

    [Required][Range(0.01, double.MaxValue)]
    public double QuantityOrdered { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}