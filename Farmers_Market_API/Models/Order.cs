using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmerMarketAPI.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }
    
    [Required]
    public int BuyerId { get; set; }
    
    [Required]
    public int ListingId { get; set; }
    
    [Required][Range(0.01, double.MaxValue)]
    public double QuantityOrdered { get; set; }
    
    public double TotalPrice { get; set; }
    
    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public DateTime? CollectionDate { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    [ForeignKey("BuyerId")]
    public virtual Buyer? Buyer { get; set; }
    
    [ForeignKey("ListingId")]
    public virtual ProduceListing? Listing { get; set; }
    
    public bool CanBeConfirmed() => Status == OrderStatus.Pending;
    public bool CanBeCollected() => Status == OrderStatus.Confirmed;
    public bool CanBeCancelled() => Status != OrderStatus.Collected;
    
    public void Confirm() { if (!CanBeConfirmed()) throw new InvalidOperationException($"Cannot confirm order with status {Status}"); Status = OrderStatus.Confirmed; }
    public void Collect() { if (!CanBeCollected()) throw new InvalidOperationException($"Cannot collect order with status {Status}"); Status = OrderStatus.Collected; CollectionDate = DateTime.Now; }
    public void Cancel() { if (!CanBeCancelled()) throw new InvalidOperationException($"Cannot cancel order with status {Status}"); Status = OrderStatus.Cancelled; }
}