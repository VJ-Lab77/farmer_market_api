using Microsoft.AspNetCore.Mvc;
using FarmerMarketAPI.Models;
using FarmerMarketAPI.Repositories;
using FarmerMarketAPI.Exceptions;

namespace FarmerMarketAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FarmerController : ControllerBase
{
    private readonly FarmerRepository _farmerRepository;
    private readonly ILogger<FarmerController> _logger;

    public FarmerController(FarmerRepository farmerRepository, ILogger<FarmerController> logger)
    {
        _farmerRepository = farmerRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Farmer>>> GetAll() => Ok(await _farmerRepository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Farmer>> GetById(int id)
    {
        try { return Ok(await _farmerRepository.GetByIdAsync(id)); }
        catch (ListingNotFoundException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost]
    public async Task<ActionResult<Farmer>> RegisterFarmer([FromBody] Farmer farmer)
    {
        if (string.IsNullOrWhiteSpace(farmer.FullName) || farmer.FullName.Length < 2)
            return BadRequest(new { error = "Full name must be at least 2 characters" });
        if (string.IsNullOrWhiteSpace(farmer.Email) || !farmer.Email.Contains("@"))
            return BadRequest(new { error = "Valid email address is required" });
        if (string.IsNullOrWhiteSpace(farmer.FarmName) || farmer.FarmName.Length < 3)
            return BadRequest(new { error = "Farm name must be at least 3 characters" });
        
        farmer.IsVerified = false;
        farmer.Rating = 0;
        var created = await _farmerRepository.AddAsync(farmer);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id}/rating")]
    public async Task<ActionResult<object>> UpdateRating(int id)
    {
        try
        {
            var newRating = await _farmerRepository.UpdateAverageRatingAsync(id);
            return Ok(new { farmerId = id, averageRating = newRating });
        }
        catch (ListingNotFoundException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpGet("{id}/reviews")]
    public async Task<ActionResult<IEnumerable<Review>>> GetReviews(int id)
    {
        try { return Ok(await _farmerRepository.GetReviewsForFarmerAsync(id)); }
        catch (ListingNotFoundException ex) { return NotFound(new { error = ex.Message }); }
    }
}