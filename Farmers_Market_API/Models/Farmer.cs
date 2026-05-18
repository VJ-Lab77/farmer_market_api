using System.ComponentModel.DataAnnotations;

namespace FarmerMarketAPI.Models;

public class Farmer : Person
{
    [Required][MinLength(3)]
    public string FarmName { get; set; } = string.Empty;
    
    [Required]
    public string Location { get; set; } = string.Empty;
    
    [Required]
    public Province Province { get; set; }
    
    [Range(0, 5)]
    public double Rating { get; set; }
    
    public bool IsVerified { get; set; }
    
    public virtual ICollection<ProduceListing> Listings { get; set; } = new List<ProduceListing>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    
    public override string GetContactInfo() => $"{FullName} from {FarmName} - {Email}, {PhoneNumber} (Province: {Province}) | Rating: {Rating:F1}★";
}