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
                    { "linux-arm", new DebPacker("linux-arm") },
                    { "linux-arm64", new DebPacker("linux-arm64") }
                }
            },
            {
                PackageType.Zip, new Dictionary<string, IOperationSystemPacker>
                {
                    { "win-x64", new ZipPacker("win-x64") },
                    { "win-arm64", new ZipPacker("win-arm64") },
                    { "win-x86", new ZipPacker("win-x86") },
                    { "win-arm", new ZipPacker("win-arm") },

                    { "linux-x64", new ZipPacker("linux-x64") },
                    { "linux-musl-x64", new ZipPacker("linux-musl-x64") },
                    { "linux-arm", new ZipPacker("linux-arm") },
                    { "linux-arm64", new ZipPacker("linux-arm64") },

                    { "osx-arm64", new ZipPacker("osx-arm64") },
                    { "osx-x64", new ZipPacker("osx-x64") },
                }
            }
        };
}