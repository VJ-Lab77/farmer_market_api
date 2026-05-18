using FarmerMarketAPI.Models;

namespace FarmerMarketAPI.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Farmers.Any())
        {
            var farmers = new[]
            {
                new Farmer { FullName = "Thabo Mbeki", Email = "thabo@greenacres.co.za", PhoneNumber = "0821234567", FarmName = "Green Acres Farm", Location = "Cullinan", Province = Province.Gauteng, Rating = 4.5, IsVerified = true },
                new Farmer { FullName = "Nomusa Dlamini", Email = "nomusa@sunrise.co.za", PhoneNumber = "0839876543", FarmName = "Sunrise Organics", Location = "Stellenbosch", Province = Province.WesternCape, Rating = 4.8, IsVerified = true }
            };
            context.Farmers.AddRange(farmers);
            context.SaveChanges();
        }

        if (!context.ProduceListings.Any())
        {
            var farmer1 = context.Farmers.First();
            var farmer2 = context.Farmers.Skip(1).First();

            var listings = new[]
            {
                new ProduceListing { FarmerId = farmer1.Id, ProductName = "Fresh Tomatoes", Category = Category.Vegetables, PricePerKg = 25.00, QuantityKg = 100, IsAvailable = true, HarvestDate = DateTime.Now.AddDays(-2), DateListed = DateTime.Now, Description = "Ripe red tomatoes" },
                new ProduceListing { FarmerId = farmer1.Id, ProductName = "Green Peppers", Category = Category.Vegetables, PricePerKg = 30.00, QuantityKg = 75, IsAvailable = true, HarvestDate = DateTime.Now.AddDays(-1), DateListed = DateTime.Now, Description = "Crisp bell peppers" },
                new ProduceListing { FarmerId = farmer2.Id, ProductName = "Strawberries", Category = Category.Fruit, PricePerKg = 80.00, QuantityKg = 50, IsAvailable = true, HarvestDate = DateTime.Now, DateListed = DateTime.Now, Description = "Sweet organic strawberries" }
            };
            context.ProduceListings.AddRange(listings);
            context.SaveChanges();
        }

        if (!context.Buyers.Any())
        {
            var buyers = new[]
            {
                new Buyer { FullName = "John Smith", Email = "john@restaurant.co.za", PhoneNumber = "0715551234", BuyerType = BuyerType.Restaurant, Location = "Johannesburg" },
                new Buyer { FullName = "Mary Jones", Email = "mary@spaza.co.za", PhoneNumber = "0725555678", BuyerType = BuyerType.SpazaShop, Location = "Pretoria" }
            };
            context.Buyers.AddRange(buyers);
            context.SaveChanges();
        }
    }
}