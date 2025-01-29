namespace Packtastic.Avalonia.Core.Factories;

public class OperationSystemPackerFactory
{
    public Dictionary<PackageType, Dictionary<string, IOperationSystemPacker>> OperationSystemPackers { get; } =
        new()
        {
            {
                PackageType.Deb, new Dictionary<string, IOperationSystemPacker>
                {
                    { "linux-x64", new DebPacker("linux-x64") },
                    { "linux-musl-x64", new DebPacker("linux-musl-x64") },
                    { "linux-arm", new DebPacker("linux-ar") },
                    { "linux-arm64", new DebPacker("linux-arm64") }
                }
            }
        };
}