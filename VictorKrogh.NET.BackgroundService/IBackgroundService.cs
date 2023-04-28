namespace VictorKrogh.NET.BackgroundServices;

public interface IBackgroundService
{
    Task ExecuteCoreAsync(CancellationToken cancellationToken);
}
