namespace FarmerMarketAPI.Exceptions;

public class ListingNotFoundException : Exception
{
    public ListingNotFoundException() : base("The requested listing was not found.") { }
    public ListingNotFoundException(string message) : base(message) { }
    public ListingNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}