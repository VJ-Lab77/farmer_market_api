using Microsoft.AspNetCore.Mvc;
using FarmerMarketAPI.Models;
using FarmerMarketAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FarmerMarketAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(AppDbContext context, ILogger<DashboardController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<object>> GetSummary()
    {
        try
        {
            var totalListings = await _context.ProduceListings.CountAsync();
            var activeListings = await _context.ProduceListings.CountAsync(p => p.IsAvailable && p.QuantityKg > 0);
            var totalFarmers = await _context.Farmers.CountAsync();
            var totalBuyers = await _context.Buyers.CountAsync();
            var pendingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending);
            var confirmedOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Confirmed);
            var collectedOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Collected);
            var cancelledOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Cancelled);
            var averagePrice = await _context.ProduceListings.Where(p => p.IsAvailable).AverageAsync(p => (double?)p.PricePerKg) ?? 0;

            var listingsByCategory = await _context.ProduceListings
                .Where(p => p.IsAvailable)
                .GroupBy(p => p.Category)
                .Select(g => new { Category = g.Key.ToString(), Count = g.Count(), AveragePrice = g.Average(p => p.PricePerKg), TotalQuantityKg = g.Sum(p => p.QuantityKg) })
                .ToListAsync();

            return Ok(new
            {
                TotalListings = totalListings,
                ActiveListings = activeListings,
                TotalFarmers = totalFarmers,
                TotalBuyers = totalBuyers,
                PendingOrders = pendingOrders,
                ConfirmedOrders = confirmedOrders,
                CollectedOrders = collectedOrders,
                CancelledOrders = cancelledOrders,
                AveragePricePerKg = averagePrice,
                ListingsByCategory = listingsByCategory,
                LastUpdated = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating dashboard summary");
            return StatusCode(500, new { error = "Failed to generate dashboard summary" });
        }
    }
}