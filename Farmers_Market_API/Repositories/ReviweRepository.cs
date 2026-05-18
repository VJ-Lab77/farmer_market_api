// Week 4 - Tue 17 May: ReviewRepository

using FarmerMarketAPI.Data;
using FarmerMarketAPI.Exceptions;
using FarmerMarketAPI.Interfaces;
using FarmerMarketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmerMarketAPI.Repositories
{
    public class ReviewRepository : IRepository<Review>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ReviewRepository> _logger;

        public ReviewRepository(AppDbContext context, ILogger<ReviewRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Buyer)
                .Include(r => r.Farmer)
                .Include(r => r.Order)
                .FirstOrDefaultAsync(r => r.ReviewId == id);
            
            if (review == null)
            {
                throw new ListingNotFoundException($"Review with ID {id} not found.");
            }
            
            return review;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews
                .Include(r => r.Buyer)
                .Include(r => r.Farmer)
                .OrderByDescending(r => r.DatePosted)
                .ToListAsync();
        }

        public async Task<Review> AddAsync(Review review)
        {
            // Check if review already exists for this order
            var existing = await _context.Reviews
                .FirstOrDefaultAsync(r => r.OrderId == review.OrderId);
            
            if (existing != null)
            {
                throw new InvalidOperationException("A review already exists for this order.");
            }
            
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review> UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task DeleteAsync(int id)
        {
            var review = await GetByIdAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Reviews.AnyAsync(r => r.ReviewId == id);
        }

        public async Task<IEnumerable<Review>> GetReviewsForFarmerAsync(int farmerId)
        {
            return await _context.Reviews
                .Include(r => r.Buyer)
                .Where(r => r.FarmerId == farmerId)
                .OrderByDescending(r => r.DatePosted)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingForFarmerAsync(int farmerId)
        {
            var ratings = await _context.Reviews
                .Where(r => r.FarmerId == farmerId)
                .Select(r => (double)r.Rating)
                .ToListAsync();
            
            return ratings.Any() ? ratings.Average() : 0;
        }
    }
}