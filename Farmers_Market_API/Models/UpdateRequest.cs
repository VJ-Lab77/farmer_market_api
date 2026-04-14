using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmers_Market_API.Models
{
    public class UpdateRequest
    {
        public required string OldName { get; set; }
        public required string NewName { get; set; }
    }
}