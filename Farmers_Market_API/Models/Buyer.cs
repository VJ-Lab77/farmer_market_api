using System.ComponentModel.DataAnnotations;

namespace FarmerMarketAPI.Models;

public class Buyer : Person
{
    [Required]
    public BuyerType BuyerType { get; set; }
    
    [Required]
    public string Location { get; set; } = string.Empty;
    
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    
    public override string GetContactInfo() => $"{FullName} ({BuyerType}) - {Email}, {PhoneNumber} - Location: {Location}";
}