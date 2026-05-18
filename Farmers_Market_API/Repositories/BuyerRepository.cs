using FarmerMarketAPI.Data;
using FarmerMarketAPI.Exceptions;
using FarmerMarketAPI.Interfaces;
using FarmerMarketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmerMarketAPI.Repositories;

public class BuyerRepository : IRepository<Buyer>
{
    private readonly AppDbContext _context;
    private readonly ILogger<BuyerRepository> _logger;

    public BuyerRepository(AppDbContext context, ILogger<BuyerRepository> logger) { _context = context; _logger = logger; }

    public async Task<Buyer?> GetByIdAsync(int id)
    {
        var buyer = await _context.Buyers.FirstOrDefaultAsync(b => b.Id == id);
        if (buyer == null) throw new ListingNotFoundException($"Buyer with ID {id} not found.");
        return buyer;
    }

    public async Task<IEnumerable<Buyer>> GetAllAsync() => await _context.Buyers.ToListAsync();
    public async Task<Buyer> AddAsync(Buyer buyer) { await _context.Buyers.AddAsync(buyer); await _context.SaveChangesAsync(); return buyer; }
    public async Task<Buyer> UpdateAsync(Buyer buyer) { _context.Buyers.Update(buyer); await _context.SaveChangesAsync(); return buyer; }
    public async Task DeleteAsync(int id) { var buyer = await GetByIdAsync(id); if (buyer != null) { _context.Buyers.Remove(buyer); await _context.SaveChangesAsync(); } }
    public async Task<bool> ExistsAsync(int id) => await _context.Buyers.AnyAsync(b => b.Id == id);
}