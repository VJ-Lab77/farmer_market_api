// Week 4 - Tue 17 May: Review model

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmerMarketAPI.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        
        [Required]
        public int BuyerId { get; set; }
        
        [Required]
        public int FarmerId { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }
        
        [MaxLength(1000)]
        public string? Comment { get; set; }
        
        public DateTime DatePosted { get; set; } = DateTime.Now;
        
        [ForeignKey("BuyerId")]
        public virtual Buyer? Buyer { get; set; }
        
        [ForeignKey("FarmerId")]
        public virtual Farmer? Farmer { get; set; }
        
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }
        
        public string GetRatingStars()
        {
            return new string('★', Rating) + new string('☆', 5 - Rating);
        }
        
        public override string ToString()
        {
            return $"Review #{ReviewId}: {Rating}★ for Farmer {FarmerId} by Buyer {BuyerId}";
        }
    }
}