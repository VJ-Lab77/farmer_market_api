using Farmers_Market_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Farmers_Market_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FarmerController : ControllerBase
    {
        // Demo in-memory list of farmers
        private static readonly List<string> farmers = new List<string> { "Ibuk", "Zeem", "Deem" };

        // GET: api/farmer
        [HttpGet]
        public ActionResult<List<string>> GetListOfFarmers()
        {
            return Ok(farmers);
        }

        // POST: api/farmer
        [HttpPost]
        public ActionResult<List<string>> CreateFarmer([FromBody] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Farmer name cannot be empty.");
            }

            farmers.Add(name);
            return Ok(farmers);
        }

        // DELETE: api/farmer?name=Ibuk
        [HttpDelete]
        public ActionResult<List<string>> Delete([FromQuery] string name)
        {
            if (farmers.Contains(name))
            {
                farmers.Remove(name);
            }

            return Ok(farmers);
        }

        // PUT: api/farmer?name=Ibuk&newName=IbukUpdated
        [HttpPut]
        public List<string> UpdateFarmers([FromBody] UpdateRequest request)
        {
            if (farmers.Contains(request.OldName))
            {
                var index = farmers.IndexOf(request.OldName);
                farmers[index] = request.NewName;
                return farmers;
            }
            else
            {
                return farmers;
            }
        }
    }
}
