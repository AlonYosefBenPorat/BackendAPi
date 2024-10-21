using DAL.Data;

namespace ApiWebApp.Services
{
    public class LogCleanupService(WebAppContext context)
    {
        private readonly WebAppContext _context = context;

        public async Task CleanupLogsAsync()
        {
            await Task.Run(async () =>
            {
                var cutoffDate = DateTime.UtcNow.AddDays(-31);
                var oldLogs = _context.LoginAttempts.Where(log => log.AttemptedAt < cutoffDate);
                _context.LoginAttempts.RemoveRange(oldLogs);
                await _context.SaveChangesAsync();
            });
        }
    }
}
