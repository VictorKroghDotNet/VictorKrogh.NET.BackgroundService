namespace VictorKrogh.NET.Hosting;

public interface IBackgroundService
{
    Task ExecuteCoreAsync(CancellationToken cancellationToken);
}
