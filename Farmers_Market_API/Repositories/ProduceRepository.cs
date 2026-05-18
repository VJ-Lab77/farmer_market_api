using FarmerMarketAPI.Data;
using FarmerMarketAPI.Exceptions;
using FarmerMarketAPI.Interfaces;
using FarmerMarketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmerMarketAPI.Repositories;

public class ProduceRepository : IRepository<ProduceListing>
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProduceRepository> _logger;

    public ProduceRepository(AppDbContext context, ILogger<ProduceRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ProduceListing?> GetByIdAsync(int id)
    {
        var listing = await _context.ProduceListings.Include(p => p.Farmer).FirstOrDefaultAsync(p => p.ListingId == id);
        if (listing == null) throw new ListingNotFoundException($"Produce listing with ID {id} not found.");
        return listing;
        
    }
    

    public async Task<IEnumerable<ProduceListing>> GetAllAsync() => await _context.ProduceListings.Include(p => p.Farmer).ToListAsync();
    public async Task<IEnumerable<ProduceListing>> GetAvailableAsync() => await _context.ProduceListings.Where(p => p.IsAvailable && p.QuantityKg > 0).ToListAsync();
    public async Task<IEnumerable<ProduceListing>> GetByCategoryAsync(Category category) => await _context.ProduceListings.Where(p => p.Category == category && p.IsAvailable).ToListAsync();
    public async Task<IEnumerable<ProduceListing>> GetByPriceRangeAsync(double minPrice, double maxPrice) => await _context.ProduceListings.Where(p => p.IsAvailable && p.PricePerKg >= minPrice && p.PricePerKg <= maxPrice).OrderBy(p => p.PricePerKg).ToListAsync();
    
    public async Task<ProduceListing> AddAsync(ProduceListing listing) { await _context.ProduceListings.AddAsync(listing); await _context.SaveChangesAsync(); return listing; }
    public async Task<ProduceListing> UpdateAsync(ProduceListing listing) { _context.ProduceListings.Update(listing); await _context.SaveChangesAsync(); return listing; }
    public async Task DeleteAsync(int id) { var listing = await GetByIdAsync(id); if (listing != null) { listing.IsAvailable = false; await UpdateAsync(listing); } }
    public async Task<bool> ExistsAsync(int id) => await _context.ProduceListings.AnyAsync(p => p.ListingId == id);
}