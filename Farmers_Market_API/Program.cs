// Week 4 - Wed 18 May: Add global exception handling middleware

using FarmerMarketAPI.Data;
using FarmerMarketAPI.Interfaces;
using FarmerMarketAPI.Middleware;
using FarmerMarketAPI.Models;
using FarmerMarketAPI.Repositories;
using FarmerMarketAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("FarmerMarketDB"));

// Register Repositories
builder.Services.AddScoped<IRepository<ProduceListing>, ProduceRepository>();
builder.Services.AddScoped<IRepository<Farmer>, FarmerRepository>();
builder.Services.AddScoped<IRepository<Buyer>, BuyerRepository>();
builder.Services.AddScoped<IRepository<Order>, OrderRepository>();
builder.Services.AddScoped<IRepository<Review>, ReviewRepository>();

// Register Services
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<INotifiable, EmailNotifier>();
builder.Services.AddScoped<SmsNotifier>();

// Additional repositories
builder.Services.AddScoped<ProduceRepository>();
builder.Services.AddScoped<FarmerRepository>();
builder.Services.AddScoped<BuyerRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<ReviewRepository>();
builder.Services.AddScoped<PriceHistoryRepository>();

var app = builder.Build();

// ✅ Week 4 - Wed 18 May: Add global exception handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedDatabase(context);
}

static void SeedDatabase(AppDbContext context)
{
    // Seed logic can be added here if needed.
}

app.Run();