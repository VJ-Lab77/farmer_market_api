using FarmerMarketAPI.Models;
using FarmerMarketAPI.Repositories;

namespace FarmerMarketAPI.Services;

public class OrderService
{
    private readonly OrderRepository _orderRepository;
    private readonly ProduceRepository _produceRepository;

    public OrderService(OrderRepository orderRepository, ProduceRepository produceRepository)
    {
        _orderRepository = orderRepository;
        _produceRepository = produceRepository;
    }

    public async Task<Order> PlaceOrderAsync(int buyerId, int listingId, double quantity, string? notes)
    {
        var listing = await _produceRepository.GetByIdAsync(listingId);
        if (listing == null) throw new Exception($"Listing {listingId} not found");
        if (!listing.IsAvailable) throw new Exception("Listing not available");
        if (quantity > listing.QuantityKg) throw new Exception($"Only {listing.QuantityKg}kg available");

        var order = new Order
        {
            BuyerId = buyerId,
            ListingId = listingId,
            QuantityOrdered = quantity,
            TotalPrice = quantity * listing.PricePerKg,
            Status = OrderStatus.Pending,
            OrderDate = DateTime.Now,
            Notes = notes
        };
        return await _orderRepository.AddAsync(order);
    }

    public async Task<Order> ConfirmOrderAsync(int orderId) { var order = await _orderRepository.GetByIdAsync(orderId); order.Confirm(); return await _orderRepository.UpdateAsync(order); }
    public async Task<Order> CollectOrderAsync(int orderId) { var order = await _orderRepository.GetByIdAsync(orderId); order.Collect(); return await _orderRepository.UpdateAsync(order); }
    public async Task<Order> CancelOrderAsync(int orderId) { var order = await _orderRepository.GetByIdAsync(orderId); order.Cancel(); return await _orderRepository.UpdateAsync(order); }
}