using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Farmers_Market_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Farmers_Market_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProduceController : ControllerBase
    {
        private List<ProduceListing> ProduceListings = 
        [
            new(1, 1, "Tomatoes", "Vegetable", 2.5, 100, true, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-3), "Freshly harvested tomatoes."),
            new(2, 2, "Strawberries", "Fruit", 3.0, 50, true, DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-2), "Sweet and juicy strawberries."),
            new(3, 3, "Carrots", "Vegetable", 1.5, 200, true, DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-4), "Crisp and fresh carrots.")
        ];

        // GET: api/produce
        [HttpGet]
        public IActionResult GetProduceListings()
        {
            return Ok(ProduceListings);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduceListingById(int id)
        {
            var produce = ProduceListings.FirstOrDefault(p => p.ListingId == id);
            if (produce == null)
            {
                return NotFound();
            }
            return Ok(produce);
        }

        [HttpGet("{id}/summary")]
        public IActionResult GetProduceListingSummary(int id)
        {
            var produce = ProduceListings.FirstOrDefault(p => p.ListingId == id);
            if (produce == null)
            {
                return NotFound();
            }
            return Ok(produce.GetFormattedSummary());
        }

        [HttpPost]
        public IActionResult CreateProduceListing([FromBody] ProduceListing newListing)
        {
            ProduceListings.Add(newListing);
            return Created($"localhost:5192/api/Produce/{newListing.ListingId}", ProduceListings);
        }
    }
}