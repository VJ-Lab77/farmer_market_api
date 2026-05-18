using FarmerMarketAPI.Interfaces;

namespace FarmerMarketAPI.Services;

public class EmailNotifier : INotifiable
{
    private readonly ILogger<EmailNotifier> _logger;
    public EmailNotifier(ILogger<EmailNotifier> logger) { _logger = logger; }
    public async Task SendNotificationAsync(string to, string subject, string message) { _logger.LogInformation("EMAIL STUB: To: {To}, Subject: {Subject}, Message: {Message}", to, subject, message); await Task.CompletedTask; }
}