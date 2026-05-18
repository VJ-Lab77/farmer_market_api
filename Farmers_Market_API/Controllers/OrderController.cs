using Microsoft.AspNetCore.Mvc;
using FarmerMarketAPI.Models;
using FarmerMarketAPI.Repositories;
using FarmerMarketAPI.Services;
using FarmerMarketAPI.Exceptions;

namespace FarmerMarketAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderRepository _orderRepository;
    private readonly ProduceRepository _produceRepository;
    private readonly ILogger<OrderController> _logger;

    public OrderController(OrderRepository orderRepository, ProduceRepository produceRepository, ILogger<OrderController> logger)
    {
        _orderRepository = orderRepository;
        _produceRepository = produceRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll() => Ok(await _orderRepository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetById(int id)
    {
        try { return Ok(await _orderRepository.GetByIdAsync(id)); }
        catch (ListingNotFoundException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        try
        {
            if (request == null) return BadRequest(new { error = "Order request cannot be empty" });
            if (request.BuyerId <= 0) return BadRequest(new { error = "Valid BuyerId is required" });
            if (request.ListingId <= 0) return BadRequest(new { error = "Valid ListingId is required" });
            if (request.QuantityOrdered <= 0) return BadRequest(new { error = "Quantity must be greater than 0" });

            var listing = await _produceRepository.GetByIdAsync(request.ListingId);
            if (listing == null) return BadRequest(new { error = $"Listing {request.ListingId} not found" });
            if (!listing.IsAvailable) return BadRequest(new { error = "This listing is no longer available" });
            if (request.QuantityOrdered > listing.QuantityKg) return BadRequest(new { error = $"Only {listing.QuantityKg}kg available" });

            var order = new Order
            {
                BuyerId = request.BuyerId,
                ListingId = request.ListingId,
                QuantityOrdered = request.QuantityOrdered,
                TotalPrice = request.QuantityOrdered * listing.PricePerKg,
                Status = OrderStatus.Pending,
                OrderDate = DateTime.Now,
                Notes = request.Notes
            };
            var created = await _orderRepository.AddAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = created.OrderId }, created);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error creating order"); return StatusCode(500, new { error = "Failed to create order" }); }
    }

    [HttpPatch("{id}/confirm")]
    public async Task<IActionResult> ConfirmOrder(int id)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return NotFound(new { error = $"Order {id} not found" });
            if (order.Status != OrderStatus.Pending) return BadRequest(new { error = $"Cannot confirm order with status {order.Status}" });
            order.Status = OrderStatus.Confirmed;
            await _orderRepository.UpdateAsync(order);
            return Ok(order);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error confirming order"); return StatusCode(500, new { error = "Failed to confirm order" }); }
    }

    [HttpPatch("{id}/collect")]
    public async Task<IActionResult> CollectOrder(int id)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return NotFound(new { error = $"Order {id} not found" });
            if (order.Status != OrderStatus.Confirmed) return BadRequest(new { error = $"Cannot collect order with status {order.Status}" });

            var listing = await _produceRepository.GetByIdAsync(order.ListingId);
            if (listing != null)
            {
                listing.QuantityKg -= order.QuantityOrdered;
                if (listing.QuantityKg <= 0) listing.IsAvailable = false;
                await _produceRepository.UpdateAsync(listing);
            }

            order.Status = OrderStatus.Collected;
            order.CollectionDate = DateTime.Now;
            await _orderRepository.UpdateAsync(order);
            return Ok(order);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error collecting order"); return StatusCode(500, new { error = "Failed to collect order" }); }
    }

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return NotFound(new { error = $"Order {id} not found" });
            if (order.Status == OrderStatus.Collected) return BadRequest(new { error = "Cannot cancel a collected order" });
            order.Status = OrderStatus.Cancelled;
            await _orderRepository.UpdateAsync(order);
            return Ok(order);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error cancelling order"); return StatusCode(500, new { error = "Failed to cancel order" }); }
    }

    [HttpGet("buyer/{buyerId}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetByBuyer(int buyerId) => Ok(await _orderRepository.GetOrdersByBuyerAsync(buyerId));

    [HttpGet("farmer/{farmerId}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetByFarmer(int farmerId) => Ok(await _orderRepository.GetOrdersByFarmerAsync(farmerId));
}