// Week 4 - Tue 17 May: PriceHistory record (track price changes)

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmerMarketAPI.Models
{
    public class PriceHistory
    {
        [Key]
        public int PriceHistoryId { get; set; }
        
        [Required]
        public int ListingId { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public double OldPrice { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public double NewPrice { get; set; }
        
        public DateTime ChangeDate { get; set; } = DateTime.Now;
        
        [ForeignKey("ListingId")]
        public virtual ProduceListing? Listing { get; set; }
        
        public double PriceDifference => NewPrice - OldPrice;
        
        public double PriceChangePercentage => OldPrice > 0 ? (PriceDifference / OldPrice) * 100 : 0;
        
        public override string ToString()
        {
            return $"Listing {ListingId}: R{OldPrice:F2} → R{NewPrice:F2} on {ChangeDate:yyyy-MM-dd} ({PriceChangePercentage:+0.00;-0.00}%)";
        }
    }
}