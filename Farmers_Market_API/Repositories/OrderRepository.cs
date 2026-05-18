using FarmerMarketAPI.Data;
using FarmerMarketAPI.Exceptions;
using FarmerMarketAPI.Interfaces;
using FarmerMarketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmerMarketAPI.Repositories;

public class OrderRepository : IRepository<Order>
{
    private readonly AppDbContext _context;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(AppDbContext context, ILogger<OrderRepository> logger) { _context = context; _logger = logger; }

    public async Task<Order?> GetByIdAsync(int id)
    {
        var order = await _context.Orders.Include(o => o.Buyer).Include(o => o.Listing).FirstOrDefaultAsync(o => o.OrderId == id);
        if (order == null) throw new ListingNotFoundException($"Order with ID {id} not found.");
        return order;
    }

    public async Task<IEnumerable<Order>> GetAllAsync() => await _context.Orders.Include(o => o.Buyer).Include(o => o.Listing).OrderByDescending(o => o.OrderDate).ToListAsync();
    public async Task<Order> AddAsync(Order order) { await _context.Orders.AddAsync(order); await _context.SaveChangesAsync(); return order; }
    public async Task<Order> UpdateAsync(Order order) { _context.Orders.Update(order); await _context.SaveChangesAsync(); return order; }
    public async Task DeleteAsync(int id) { var order = await GetByIdAsync(id); if (order != null) { _context.Orders.Remove(order); await _context.SaveChangesAsync(); } }
    public async Task<bool> ExistsAsync(int id) => await _context.Orders.AnyAsync(o => o.OrderId == id);
    public async Task<IEnumerable<Order>> GetOrdersByBuyerAsync(int buyerId) => await _context.Orders.Where(o => o.BuyerId == buyerId).OrderByDescending(o => o.OrderDate).ToListAsync();
    public async Task<IEnumerable<Order>> GetOrdersByFarmerAsync(int farmerId) => await _context.Orders.Where(o => o.Listing != null && o.Listing.FarmerId == farmerId).OrderByDescending(o => o.OrderDate).ToListAsync();
}