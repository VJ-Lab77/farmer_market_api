// Week 4 - Tue 17 May: PriceHistoryRepository

using FarmerMarketAPI.Data;
using FarmerMarketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmerMarketAPI.Repositories
{
    public class PriceHistoryRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PriceHistoryRepository> _logger;

        public PriceHistoryRepository(AppDbContext context, ILogger<PriceHistoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddPriceHistoryAsync(int listingId, double oldPrice, double newPrice)
        {
            var history = new PriceHistory
            {
                ListingId = listingId,
                OldPrice = oldPrice,
                NewPrice = newPrice,
                ChangeDate = DateTime.Now
            };
            
            await _context.PriceHistories.AddAsync(history);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Price history added for listing {ListingId}: {OldPrice} → {NewPrice}", 
                listingId, oldPrice, newPrice);
        }

        public async Task<IEnumerable<PriceHistory>> GetPriceHistoryForListingAsync(int listingId)
        {
            return await _context.PriceHistories
                .Where(ph => ph.ListingId == listingId)
                .OrderByDescending(ph => ph.ChangeDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PriceHistory>> GetPriceHistoryByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.PriceHistories
                .Include(ph => ph.Listing)
                .Where(ph => ph.ChangeDate >= startDate && ph.ChangeDate <= endDate)
                .OrderByDescending(ph => ph.ChangeDate)
                .ToListAsync();
        }
    }
}