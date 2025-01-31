using System.Diagnostics;
using System.Text.RegularExpressions;
using NineDigit.WixSharpExtensions;
using Packtastic.Avalonia.Core.Helpers;
using WixSharp;
using File = WixSharp.File;

namespace Packtastic.Avalonia.Core.Factories;

public class MsiPacker(string platform) : IOperationSystemPacker
{
    public void Pack(IPackOptions packOptions)
    {
        if (packOptions is not IExternalPackOptions packOptionsExternal)
        {
            throw new InvalidOperationException("External pack options are required.");
        }
        
        if (!OperatingSystem.IsWindows())
        {
            throw new InvalidOperationException("Windows MSI packer is not implemented yet.");
        }

        CheckWix();

        var directory = Path.Combine(packOptionsExternal.BinaryDirectory, "bin", "Packtastic", platform);

        if (!Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException($"The directory '{directory}' does not exist.");
        }
        
        var files = Directory
            .GetFiles(directory, "*.*", SearchOption.AllDirectories)
            .Select(filePath => new FileInfo(filePath))
            .Select(fileInfo =>
                new File(new Id(GenerateComponentName(fileInfo)), fileInfo.FullName))
            .ToArray();

        var project = new Project(packOptionsExternal.Name,
            new Dir(
                $@"%ProgramFiles%\{packOptionsExternal.CompanyName}\{packOptionsExternal.Name}",
                files
            ))
        {
            GUID = new Guid("6f330b47-2577-43ad-9095-1861ba25889b"),
            Version = new Version(packOptionsExternal.Version),
        };

        if (!string.IsNullOrEmpty(packOptionsExternal.BackgroundImagePath) && System.IO.File.Exists(packOptionsExternal.BackgroundImagePath))
        {
            project.BackgroundImage = packOptionsExternal.BackgroundImagePath;
        }

        if (!string.IsNullOrEmpty(packOptionsExternal.BannerImagePath) && System.IO.File.Exists(packOptionsExternal.BannerImagePath))
        {
            project.BannerImage = packOptionsExternal.BannerImagePath;
        }

        if (!string.IsNullOrEmpty(packOptionsExternal.LicenseRtfPath))
        {
            project.LicenceFile = packOptionsExternal.LicenseRtfPath;
        }

        if (!string.IsNullOrEmpty(packOptionsExternal.ShortCutFileName) &&
            project.FindFile(f => f.Name.EndsWith(packOptionsExternal.ShortCutFileName))
                .FirstOrDefault() is {} file)
        {
            file.Shortcuts =
            [
                new FileShortcut(packOptionsExternal.ShortCutFileName, "INSTALLDIR"),
                new FileShortcut(packOptionsExternal.ShortCutFileName, "%Desktop%")
            ];
        }

        Compiler.BuildMsi(project);
    }

    private string GenerateComponentName(FileInfo fileInfo)
    {
        return $"{fileInfo.Directory!.Name.Substring(0, 2)}.{fileInfo.Name}_{fileInfo.Length}".Replace("-", "_");
    }

    private void CheckWix()
    {
        // var directory = Environment.ExpandEnvironmentVariables(WixTools.WixSharpToolDir.Replace("%userprofile%", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)));
        //
        // if (!Directory.Exists(directory))
        // {
        //     System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("dotnet", "tool install --global wix") { UseShellExecute = true })!.WaitForExit();
        // }
    }
}