using FarmerMarketAPI.Interfaces;

namespace FarmerMarketAPI.Services;

public class SmsNotifier : INotifiable
{
    private readonly ILogger<SmsNotifier> _logger;
    public SmsNotifier(ILogger<SmsNotifier> logger) { _logger = logger; }
    public async Task SendNotificationAsync(string to, string subject, string message) { _logger.LogInformation("SMS STUB: To: {To}, Message: {Message}", to, message); await Task.CompletedTask; }
}