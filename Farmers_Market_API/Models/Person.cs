using System.ComponentModel.DataAnnotations;

namespace FarmerMarketAPI.Models;

public abstract class Person
{
    [Key]
    public int Id { get; set; }
    
    [Required][MinLength(2)]
    public string FullName { get; set; } = string.Empty;
    
    [Required][EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required][Phone]
    public string PhoneNumber { get; set; } = string.Empty;
    
    public virtual string GetContactInfo() => $"{FullName} - {Email}, {PhoneNumber}";
}