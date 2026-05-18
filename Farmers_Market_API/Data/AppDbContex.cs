// ============================================================
// Week 5 - Wed 27 May: EF Core In-Memory DB (stretch goal)
// ============================================================

using FarmerMarketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmerMarketAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Farmer> Farmers { get; set; }
    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<ProduceListing> ProduceListings { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<PriceHistory> PriceHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Unique email constraints
        modelBuilder.Entity<Farmer>().HasIndex(f => f.Email).IsUnique();
        modelBuilder.Entity<Buyer>().HasIndex(b => b.Email).IsUnique();
        
        // One review per order constraint
        modelBuilder.Entity<Review>().HasIndex(r => r.OrderId).IsUnique();
    }
}