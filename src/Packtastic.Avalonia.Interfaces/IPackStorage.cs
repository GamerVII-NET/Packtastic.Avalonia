namespace Packtastic.Avalonia;

public interface IPackStorage
{
    IConfiguration Configuration { get; }
    Task SaveConfigurationAsync(CancellationToken token = default);
    Task RestoreConfigurationAsync(CancellationToken token = default);
}