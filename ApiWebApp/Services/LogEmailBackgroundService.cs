using ApiWebApp.Services.Interfaces;
using DAL.Data;
using System.Text;

namespace ApiWebApp.Services
{
    public class LogEmailBackgroundService(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await SendLogsAsync();
                await Task.Delay(TimeSpan.FromDays(7), stoppingToken);
            }
        }

        private async Task SendLogsAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WebAppContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var logs = context.LoginAttempts.ToList();
            var logText = new StringBuilder();

            foreach (var log in logs)
            {
                logText.AppendLine($"{log.AttemptedAt}: {log.UserName} - {log.IsSucceeded} - {log.RemoteIpAddress}");
            }

            await emailService.SendEmailAsync("admin@example.com", "Weekly Logs", logText.ToString());
        }
    }
}
