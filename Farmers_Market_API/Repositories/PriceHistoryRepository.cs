using FarmerMarketAPI.Data;
using FarmerMarketAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FarmerMarketAPI.Repositories;

public class PriceHistoryRepositoryImpl
{
    private readonly AppDbContext _context;
    private readonly ILogger<PriceHistoryRepositoryImpl> _logger;

    public PriceHistoryRepositoryImpl(AppDbContext context, ILogger<PriceHistoryRepositoryImpl> logger)
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
        
        _logger.LogInformation("Price history added for listing {ListingId}: {OldPrice:F2} → {NewPrice:F2}", 
            listingId, oldPrice, newPrice);
    }

    // ✅ This is the method name - keep it as is
    public async Task<IEnumerable<PriceHistory>> GetPriceHistoryForListingAsync(int listingId)
    {
        return await _context.PriceHistories
            .Where(ph => ph.ListingId == listingId)
            .OrderByDescending(ph => ph.ChangeDate)
            .ToListAsync();
    }
}