using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Infrastructure.Services;

public interface IDatabaseService
{
    Task<bool> IsDatabaseConnectedAsync();
    Task<int> GetRecordCountAsync<T>() where T : class;
    Task<bool> BackupDatabaseAsync(string backupPath);
}

public class DatabaseService : IDatabaseService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DatabaseService> _logger;

    public DatabaseService(ApplicationDbContext context, ILogger<DatabaseService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> IsDatabaseConnectedAsync()
    {
        try
        {
            await _context.Database.CanConnectAsync();
            _logger.LogInformation("Database connection successful");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database connection failed");
            return false;
        }
    }

    public async Task<int> GetRecordCountAsync<T>() where T : class
    {
        try
        {
            var count = await _context.Set<T>().CountAsync();
            _logger.LogInformation("Record count for {Type}: {Count}", typeof(T).Name, count);
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get record count for {Type}", typeof(T).Name);
            return 0;
        }
    }

    public async Task<bool> BackupDatabaseAsync(string backupPath)
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync($"BACKUP DATABASE [{_context.Database.GetDbConnection().Database}] TO DISK = '{backupPath}'");
            _logger.LogInformation("Database backup created at: {BackupPath}", backupPath);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create database backup");
            return false;
        }
    }
}
