using Farmers_Market_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Farmers_Market_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FarmerController : ControllerBase
    {
        // Demo in-memory list of farmers
        private static readonly List<Farmer> farmers = new List<Farmer> { new Farmer("Ibuk", "ibuk@example.com", "123-456-7890", "Location A", "Province A", 4.5, true), new Farmer("Wave Lee", "wave.lee@example.com", "123-456-7891", "Location B", "Province B", 4.0, true), new Farmer("Kaz", "kaz@example.com", "123-456-7892", "Location C", "Province C", 4.8, true) };

        // GET: api/farmer
        [HttpGet]
        public IActionResult GetListOfFarmers()
        {
            return Ok(farmers);
        }

        // POST: api/farmer
        [HttpPost]
        public IActionResult CreateFarmer([FromBody] Farmer farmer)
        {
            farmers.Add(farmer);
            return Ok(farmers);
        }

        // DELETE: api/farmer?farmerId=1
        [HttpDelete]
        public IActionResult Delete([FromQuery] int farmerId)
        {
            var farmer = farmers.FirstOrDefault(f => f.GetFarmerId() == farmerId);
        if (farmer != null)
            {
                farmers.Remove(farmer);
            }

            return Ok(farmers);
}       

        //PUT: api/farmer?name=Ibuk&newName=IbukUpdated
        [HttpPut]
        public IActionResult UpdateFarmers([FromBody] Farmer updatedFarmer)
        {
            var farmer = farmers.FirstOrDefault(f => f.GetFarmerId() == updatedFarmer.GetFarmerId());
            if (farmer != null)
            {
                farmers.Remove(farmer);
                return Ok(farmers);
            }

            else
            {
                return NotFound();
            }
        }
    }
}
