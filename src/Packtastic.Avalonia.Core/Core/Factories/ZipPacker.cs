using System.IO.Compression;

namespace Packtastic.Avalonia.Core.Factories;

public class ZipPacker(string platform) : IOperationSystemPacker
{
    public void Pack(IPackOptions packOptions)
    {
        var directory = Path.Combine(packOptions.BinaryDirectory, "bin", "Packtastic", platform);

        if (!Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException($"The directory '{directory}' does not exist.");
        }

        var zipDirectory = Path.Combine(directory, "Zip");
        if (!Directory.Exists(zipDirectory))
        {
            Directory.CreateDirectory(zipDirectory);
        }

        var zipFileName = $"{packOptions.SlugName}-{platform}.zip";
        var zipFilePath = Path.Combine(zipDirectory, zipFileName);

        if (File.Exists(zipFilePath))
        {
            File.Delete(zipFilePath);
        }

        ZipFile.CreateFromDirectory(directory, zipFilePath);

        Console.WriteLine($"Packed {platform} binary into: {zipFilePath}");
    }
}