using Microsoft.AspNetCore.Mvc;
using FarmerMarketAPI.Models;
using FarmerMarketAPI.Repositories;
using FarmerMarketAPI.Exceptions;

namespace FarmerMarketAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuyerController : ControllerBase
{
    private readonly BuyerRepository _buyerRepository;
    private readonly ILogger<BuyerController> _logger;

    public BuyerController(BuyerRepository buyerRepository, ILogger<BuyerController> logger)
    {
        _buyerRepository = buyerRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Buyer>>> GetAll() => Ok(await _buyerRepository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Buyer>> GetById(int id)
    {
        try { return Ok(await _buyerRepository.GetByIdAsync(id)); }
        catch (ListingNotFoundException ex) { return NotFound(new { error = ex.Message }); }
    }

    [HttpPost]
    public async Task<ActionResult<Buyer>> RegisterBuyer([FromBody] Buyer buyer)
    {
        if (string.IsNullOrWhiteSpace(buyer.FullName) || buyer.FullName.Length < 2)
            return BadRequest(new { error = "Full name must be at least 2 characters" });
        if (string.IsNullOrWhiteSpace(buyer.Email) || !buyer.Email.Contains("@"))
            return BadRequest(new { error = "Valid email address is required" });
        
        var created = await _buyerRepository.AddAsync(buyer);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}