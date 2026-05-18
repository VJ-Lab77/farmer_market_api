using Microsoft.AspNetCore.Mvc;
using FarmerMarketAPI.Models;
using FarmerMarketAPI.Repositories;
using FarmerMarketAPI.Exceptions;

namespace FarmerMarketAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly ReviewRepository _reviewRepository;
    private readonly FarmerRepository _farmerRepository;
    private readonly OrderRepository _orderRepository;
    private readonly ILogger<ReviewController> _logger;

    public ReviewController(ReviewRepository reviewRepository, FarmerRepository farmerRepository, OrderRepository orderRepository, ILogger<ReviewController> logger)
    {
        _reviewRepository = reviewRepository;
        _farmerRepository = farmerRepository;
        _orderRepository = orderRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> GetAll() => Ok(await _reviewRepository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Review>> GetById(int id)
    {
        try { return Ok(await _reviewRepository.GetByIdAsync(id)); }
        catch (ListingNotFoundException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost]
    public async Task<ActionResult<Review>> CreateReview([FromBody] CreateReviewRequest request)
    {
        try
        {
            if (request == null) return BadRequest(new { error = "Review request cannot be empty" });
            if (request.Rating < 1 || request.Rating > 5) return BadRequest(new { error = "Rating must be between 1 and 5" });

            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null) return BadRequest(new { error = $"Order {request.OrderId} not found" });
            if (order.Status != OrderStatus.Collected) return BadRequest(new { error = "Can only review orders that have been collected" });
            if (order.BuyerId != request.BuyerId) return BadRequest(new { error = "You can only review your own orders" });

            var review = new Review
            {
                BuyerId = request.BuyerId,
                FarmerId = request.FarmerId,
                OrderId = request.OrderId,
                Rating = request.Rating,
                Comment = request.Comment,
                DatePosted = DateTime.Now
            };
            var created = await _reviewRepository.AddAsync(review);
            await _farmerRepository.UpdateAverageRatingAsync(request.FarmerId);
            return CreatedAtAction(nameof(GetById), new { id = created.ReviewId }, created);
        }
        catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }
        catch (Exception ex) { _logger.LogError(ex, "Error creating review"); return StatusCode(500, new { error = "Failed to create review" }); }
    }

    [HttpGet("farmer/{farmerId}")]
    public async Task<ActionResult<IEnumerable<Review>>> GetByFarmer(int farmerId) => Ok(await _reviewRepository.GetReviewsForFarmerAsync(farmerId));
}

public class CreateReviewRequest
{
    public int BuyerId { get; set; }
    public int FarmerId { get; set; }
    public int OrderId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}