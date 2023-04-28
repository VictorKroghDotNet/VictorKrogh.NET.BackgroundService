using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VictorKrogh.NET.BackgroundServices;

public class HostedService<T> : BackgroundService
    where T : IBackgroundService
{
    public HostedService(ILogger<HostedService<T>> logger, IServiceProvider serviceProvider)
    {
        Logger = logger;
        ServiceProvider = serviceProvider;
    }

    protected ILogger<HostedService<T>> Logger { get; }
    protected IServiceProvider ServiceProvider { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await ExecuteCoreAsync(stoppingToken);
        }
        catch (TaskCanceledException ex)
        {
            Logger.LogWarning(ex, $"Hosted service for '{typeof(T).Name}' stopped execution of tasks.");
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, $"Hosted service for '{typeof(T).Name}' failed during execution of tasks.");
            throw;
        }
    }

    protected virtual async Task ExecuteCoreAsync(CancellationToken stoppingToken)
    {
        using var scope = ServiceProvider.CreateScope();

        var backgroundService = scope.ServiceProvider.GetService<T>();
        if (backgroundService == null)
        {
            Logger.LogError($"Background Service '{typeof(T)}' was not found. Might not be registered.");
            return;
        }

        await backgroundService.ExecuteCoreAsync(stoppingToken);
    }
}
