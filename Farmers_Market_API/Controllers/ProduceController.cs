using Microsoft.AspNetCore.Mvc;
using FarmerMarketAPI.Models;
using FarmerMarketAPI.Repositories;
using FarmerMarketAPI.Exceptions;

namespace FarmerMarketAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProduceController : ControllerBase
{
    private readonly ProduceRepository _repository;
    private readonly PriceHistoryRepository _priceHistoryRepository;
    private readonly ILogger<ProduceController> _logger;

    public ProduceController(
        ProduceRepository repository, 
        PriceHistoryRepository priceHistoryRepository,
        ILogger<ProduceController> logger)
    {
        _repository = repository;
        _priceHistoryRepository = priceHistoryRepository;
        _logger = logger;
    }

    // GET: api/produce
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProduceListing>>> GetAll()
    {
        var listings = await _repository.GetAllAsync();
        return Ok(listings);
    }

    // GET: api/produce/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProduceListing>> GetById(int id)
    {
        try
        {
            var listing = await _repository.GetByIdAsync(id);
            return Ok(listing);
        }
        catch (ListingNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    // GET: api/produce/available
    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<ProduceListing>>> GetAvailable()
    {
        var listings = await _repository.GetAvailableAsync();
        return Ok(listings);
    }

    // GET: api/produce/category/Vegetables
    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<ProduceListing>>> GetByCategory(Category category)
    {
        var listings = await _repository.GetByCategoryAsync(category);
        return Ok(listings);
    }

    // GET: api/produce/price-range?minPrice=10&maxPrice=50
    [HttpGet("price-range")]
    public async Task<ActionResult<IEnumerable<ProduceListing>>> GetByPriceRange(
        [FromQuery] double minPrice, 
        [FromQuery] double maxPrice)
    {
        if (minPrice < 0)
        {
            return BadRequest(new { error = "Minimum price cannot be negative" });
        }
        
        if (maxPrice < minPrice)
        {
            return BadRequest(new { error = "Maximum price must be greater than minimum price" });
        }
        
        var listings = await _repository.GetByPriceRangeAsync(minPrice, maxPrice);
        return Ok(listings);
    }

    // POST: api/produce
    [HttpPost]
    public async Task<ActionResult<ProduceListing>> CreateListing([FromBody] ProduceListing listing)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(listing.ProductName) || listing.ProductName.Length < 3)
            {
                return BadRequest(new { error = "Product name must be at least 3 characters" });
            }
            
            if (listing.PricePerKg <= 0)
            {
                return BadRequest(new { error = "Price must be greater than 0" });
            }
            
            if (listing.QuantityKg <= 0)
            {
                return BadRequest(new { error = "Quantity must be greater than 0" });
            }
            
            var created = await _repository.AddAsync(listing);
            return CreatedAtAction(nameof(GetById), new { id = created.ListingId }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating listing");
            return StatusCode(500, new { error = "Failed to create listing" });
        }
    }

    // PUT: api/produce/5
    [HttpPut("{id}")]
    public async Task<ActionResult<ProduceListing>> UpdateListing(int id, [FromBody] ProduceListing updatedListing)
    {
        try
        {
            if (id != updatedListing.ListingId)
            {
                return BadRequest(new { error = "ID mismatch" });
            }
            
            var existingListing = await _repository.GetByIdAsync(id);
            
            // Track price changes for history
            if (existingListing.PricePerKg != updatedListing.PricePerKg)
            {
                await _priceHistoryRepository.AddPriceHistoryAsync(
                    id, 
                    existingListing.PricePerKg, 
                    updatedListing.PricePerKg);
            }
            
            var result = await _repository.UpdateAsync(updatedListing);
            return Ok(result);
        }
        catch (ListingNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating listing {Id}", id);
            return StatusCode(500, new { error = "Failed to update listing" });
        }
    }

    // DELETE: api/produce/5 (soft delete)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteListing(int id)
    {
        try
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
        catch (ListingNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting listing {Id}", id);
            return StatusCode(500, new { error = "Failed to delete listing" });
        }
    }

    // GET: api/produce/5/price-history
    [HttpGet("{id}/price-history")]
    public async Task<ActionResult<IEnumerable<PriceHistory>>> GetPriceHistory(int id)
    {
        try
        {
            await _repository.GetByIdAsync(id); // Verify listing exists
            
            // ✅ FIXED: Changed from GetPriceHistoryAsync to GetPriceHistoryForListingAsync
            var history = await _priceHistoryRepository.GetPriceHistoryForListingAsync(id);
            
            return Ok(history);
        }
        catch (ListingNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}