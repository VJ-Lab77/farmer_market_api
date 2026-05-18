// ============================================================
// Week 2 - Wed 22 April: FarmerRepository stub
// Week 3 - Mon 11 May: FarmerRepository implements IRepository<Farmer> (Interface reuse)
// Week 4 - Tue 17 May: Add UpdateAverageRatingAsync and GetReviewsForFarmerAsync
// ============================================================

using FarmerMarketAPI.Data;
using FarmerMarketAPI.Exceptions;
using FarmerMarketAPI.Interfaces;
using FarmerMarketAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace FarmerMarketAPI.Repositories;

public class FarmerRepository : IRepository<Farmer>
{
    private readonly AppDbContext _context;
    private readonly ILogger<FarmerRepository> _logger;

    public FarmerRepository(AppDbContext context, ILogger<FarmerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Farmer?> GetByIdAsync(int id)
    {
        var farmer = await _context.Farmers
            .Include(f => f.Listings)
            .FirstOrDefaultAsync(f => f.Id == id);
        
        if (farmer == null)
            throw new ListingNotFoundException($"Farmer with ID {id} not found.");
        
        return farmer;
    }

    public async Task<IEnumerable<Farmer>> GetAllAsync()
    {
        return await _context.Farmers.ToListAsync();
    }

    public async Task<Farmer> AddAsync(Farmer farmer)
    {
        await _context.Farmers.AddAsync(farmer);
        await _context.SaveChangesAsync();
        return farmer;
    }

    public async Task<Farmer> UpdateAsync(Farmer farmer)
    {
        _context.Farmers.Update(farmer);
        await _context.SaveChangesAsync();
        return farmer;
    }

    public async Task DeleteAsync(int id)
    {
        var farmer = await GetByIdAsync(id);
        if (farmer != null)
        {
            _context.Farmers.Remove(farmer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Farmers.AnyAsync(f => f.Id == id);
    }

    // ✅ Week 4 - Tue 17 May: Update farmer's average rating based on reviews
    // FIXED: Changed from private to public
    public async Task<double> UpdateAverageRatingAsync(int farmerId)
    {
        var farmer = await GetByIdAsync(farmerId);
        if (farmer == null)
        {
            throw new ListingNotFoundException($"Farmer with ID {farmerId} not found.");
        }
        
        var reviews = await _context.Reviews
            .Where(r => r.FarmerId == farmerId)
            .ToListAsync();
        
        if (reviews.Any())
        {
            farmer.Rating = reviews.Average(r => r.Rating);
            await UpdateAsync(farmer);
        }
        
        return farmer.Rating;
    }

    // ✅ Week 4 - Tue 17 May: Get all reviews for a farmer
    // FIXED: Changed from private to public
    public async Task<IEnumerable<Review>> GetReviewsForFarmerAsync(int farmerId)
    {
        return await _context.Reviews
            .Include(r => r.Buyer)
            .Where(r => r.FarmerId == farmerId)
            .OrderByDescending(r => r.DatePosted)
            .ToListAsync();
    }
}