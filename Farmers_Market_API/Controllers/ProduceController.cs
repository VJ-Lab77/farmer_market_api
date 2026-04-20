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
            new(3, 3, "Carrots", "Vegetable", 1.5, 200, true, DateTime.Now.AddDays(-6), DateTime.Now.AddDays(-4), "Crisp and fresh carrots."),
            new(4, 1, "Apples", "Fruit", 2.0, 75, true, DateTime.Now.AddDays(-3), DateTime.Now.AddDays(-1), "Crisp red apples."),
            new(5, 2, "Lettuce", "Vegetable", 1.8, 120, true, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1), "Fresh green lettuce leaves."),
            new(6, 3, "Potatoes", "Vegetable", 1.2, 150, true, DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-5), "Organic potatoes."),
            new(7, 1, "Blueberries", "Fruit", 4.5, 30, true, DateTime.Now.AddDays(-3), DateTime.Now.AddDays(-1), "Wild blueberries."),
            new(8, 2, "Broccoli", "Vegetable", 2.2, 80, true, DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-2), "Nutritious broccoli florets."),
            new(9, 3, "Oranges", "Fruit", 2.8, 60, true, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-3), "Juicy navel oranges."),
            new(10, 1, "Spinach", "Vegetable", 2.0, 90, true, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1), "Fresh spinach leaves.")
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

        [HttpGet("available")]
        public IActionResult getAvailableProduce()
        {
            // var availableProduce = ProduceListings.Where(p => p.IsAvailable);
            // return Ok(availableProduce);
            List<ProduceListing> availableProduce = new List<ProduceListing>();
            for (int i = 0; i < ProduceListings.Count; i++)
            {
                if (ProduceListings[i].IsAvailable)
                {
                    availableProduce.Add(ProduceListings[i]);
                }
            }
            return Ok(availableProduce);

        }
        [HttpGet("category/{category}")]
        public IActionResult getProduceByCategory([FromRoute] string category)
        {
            
            return Ok(ProduceListings.Where(produce => produce.Category == category).ToList() );
        }
    }
}