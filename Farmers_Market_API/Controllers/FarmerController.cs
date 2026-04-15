using Farmers_Market_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Farmers_Market_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FarmerController : ControllerBase
    {
        // Demo in-memory list of farmers
        private static readonly List<Farmer> farmers = new List<farmer> { new Farmer("Ibuk", "ibuk@example.com", "123-456-7890", "Location A", "Province A", 4.5, true), new Farmer("Zeem", "zeem@example.com", "123-456-7891", "Location B", "Province B", 4.0, true), new Farmer("Deem", "deem@example.com", "123-456-7892", "Location C", "Province C", 4.8, true) };

        // GET: api/farmer
        [HttpGet]
        public ActionResult<List<Farmer>> GetListOfFarmers()
        {
            return Ok(farmers);
        }

        // POST: api/farmer
        [HttpPost]
        public ActionResult<List<Farmer>> CreateFarmer([FromBody] Farmer farmer)
        {
            farmers.Add(farmer);
            return Ok(farmers);
        }

        // DELETE: api/farmer?name=Ibuk
        [HttpDelete]
        public ActionResult<List<Farmer>> Delete([FromQuery] string name)
        {
            var farmer = farmers.FirstOrDefault(f => f.FarmerId == name);
            if (farmer != null)
            {
                farmers.Remove(farmer);
            }

            return Ok(farmers);
        }

        // PUT: api/farmer?name=Ibuk&newName=IbukUpdated
        // [HttpPut]
        // public List<Farmer> UpdateFarmers([FromBody] UpdateRequest request)
        // {
        //     if (farmers.Contains(request.OldName))
        //     {
        //         var index = farmers.IndexOf(request.OldName);
        //         farmers[index] = request.NewName;
        //         return farmers;
        //     }
        //     else
        //     {
        //         return farmers;
        //     }
        //}
    }
}
