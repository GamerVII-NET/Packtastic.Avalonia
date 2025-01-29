namespace Packtastic.Avalonia.Core.Factories;

public class OperationSystemBuilderFactory
{
    public Dictionary<string, IOperationSystemBuilder> OperationSystemBuilders { get; } =
        new()
        {
            { "win-x64", new WindowsSystemBuilder("x64") },
            { "win-arm64", new WindowsSystemBuilder("arm64") },
            { "win-x86", new WindowsSystemBuilder("x86") },
            { "win-arm", new WindowsSystemBuilder("arm") },
            
            { "linux-x64", new LinuxSystemBuilder("x64") },
            { "linux-musl-x64", new LinuxSystemBuilder("musl-x64") },
            { "linux-arm", new LinuxSystemBuilder("arm") },
            { "linux-arm64", new LinuxSystemBuilder("arm64") },
        };
}