namespace FarmerMarketAPI.Interfaces;

public interface INotifiable
{
    Task SendNotificationAsync(string to, string subject, string message);
}