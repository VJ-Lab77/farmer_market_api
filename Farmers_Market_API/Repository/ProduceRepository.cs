using System;
using Farmers_Market_API.Models;
using System.Linq;
using Farmers_Market_API.Enums;
namespace Farmers_Market_API.Repository

{
    public class ProduceRepository
    {
        private List<ProduceListing> ProduceListings = new();

        public void addProduceListing(ProduceListing produce)
        {
            int newId = ProduceListings.Count > 0 ? ProduceListings.Max(static l => l.Id) + 1 : 1;
            ProduceListings.Add(produce);
        }

        public ProduceListing? GetById(int id)
        {
            for (int i = 0; i < ProduceListings.Count; i++)
            {
                if (ProduceListings[i].Id == id)
                {
                    return ProduceListings[i];
                }
            }

            return null;
        }

        public List<ProduceListing> GetByCategory(Category category)
        {
            List<ProduceListing> result = new();
            for (int i = 0; i < ProduceListings.Count; i++)
            {
                if (ProduceListings[i].Category == category)
                {
                    result.Add(ProduceListings[i]);
                }
            }
            return result;
        }

        public List<ProduceListing> GetAvailable()
            {
                List<ProduceListing> result = new();
                for (int i = 0; i < ProduceListings.Count; i++)
                {
                    if (ProduceListings[i].IsAvailable)
                    {
                        result.Add(ProduceListings[i]);
                    }
                }
                return result;
            }
        

    public class InvalidProduceFormatException : Exception
    {
        public InvalidProduceFormatException(string message) : base(message)
        {
        }
    }

    public ProduceListing AddProduce(ProduceListing produce)
    {
        // Validation checks
        if (string.IsNullOrWhiteSpace(produce.ProductName))
        {
            throw new InvalidProduceFormatException("Produce name is invalid: it cannot be null or empty.");
        }

        if (produce.PricePerKg < 0)
        {
            throw new InvalidProduceFormatException("Produce price per kg is invalid: it cannot be negative.");
        }

        if (produce.QuantityKg < 0)
        {
            throw new InvalidProduceFormatException("Produce quantity in kg is invalid: it cannot be negative.");
        }

        // Assign new ID
        int newId = ProduceListings.Any() ? ProduceListings.Max(static p => p.Id) + 1 : 1;

        ProduceListings.Add(produce);
        return produce;
    }
}
}
