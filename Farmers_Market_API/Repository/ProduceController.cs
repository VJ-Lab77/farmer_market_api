using System;
using System.Collections.Generic;   
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Farmers_Market_API.Models;
using Farmers_Market_API.Repository;
namespace Farmers_Market_API.Controllers
{
    [Route("[controller]")]
    public class ProduceController : Controller
    {
        private readonly ProduceRepository _repository;
        public ProduceController(ProduceRepository repository)
        {
            _repository = repository;
        }
        [HttpPost]
        public IActionResult AddProduceListing([FromBody] ProduceListing produce)
        {
            if (produce == null)
            {
                return BadRequest("Produce listing cannot be null.");
            }
            _repository.addProduceListing(produce);
            return CreatedAtAction(nameof(GetById), new { id = produce.Id }, produce);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var listing = _repository.GetById(id);
            if (listing == null)
            {
                return NotFound($"No produce listing found with ID {id}.");
            }
            return Ok(listing);
        }
        [HttpGet("category/{category}")]
        public IActionResult GetByCategory(string category)
        {
            if (!Enum.TryParse(category, true, out Category parsedCategory))
            {
                return BadRequest($"Invalid category: {category}. Valid categories are: {string.Join(", ", Enum.GetNames(typeof(Category)))}");
            }
            var listings = _repository.GetByCategory(parsedCategory);
            return Ok(listings);
        }
        [HttpGet("available")]
        public IActionResult GetAvailable()
        {
            var listings = _repository.GetAvailable();
            return Ok(listings);
        }

    }
}