using System.Diagnostics;
using System.Text;

namespace Packtastic.Avalonia.Core.Factories;

public class DebPacker(string platform) : IOperationSystemPacker
{
    public void Pack(IPackOptions packOptions)
    {
        var directory = Path.Combine(packOptions.BinaryDirectory, "bin", "Packtastic", platform);

        if (!Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException(directory);
        }

        var binary = Path.Combine(directory, packOptions.Name);

        if (!File.Exists(binary))
        {
            throw new FileNotFoundException(binary);
        }
        
        var stagingFolder = Path.Combine(directory, "staging_folder");
        var debianFolder = Path.Combine(stagingFolder, "DEBIAN");
        var usrBinFolder = Path.Combine(stagingFolder, "usr", "bin");
        var usrLibFolder = Path.Combine(stagingFolder, "usr", "lib", packOptions.SlugName);
        var applicationsFolder = Path.Combine(stagingFolder, "usr", "share", "applications");
        var iconsFolder = Path.Combine(stagingFolder, "usr", "share", "icons", "hicolor", "scalable", "apps");
        var pixmapsFolder = Path.Combine(stagingFolder, "usr", "share", "pixmaps");
        
        Directory.CreateDirectory(debianFolder);
        Directory.CreateDirectory(usrBinFolder);
        Directory.CreateDirectory(usrLibFolder);
        Directory.CreateDirectory(applicationsFolder);
        Directory.CreateDirectory(iconsFolder);
        Directory.CreateDirectory(pixmapsFolder);

        string controlFile = Path.Combine(debianFolder, "control");
        File.WriteAllText(controlFile, GenerateControlFile(packOptions), new UTF8Encoding(false));

        foreach (var file in Directory.GetFiles(directory))
        {
            File.Copy(file, Path.Combine(usrLibFolder, Path.GetFileName(file)), true);
        }

        string starterScript = Path.Combine(usrBinFolder, packOptions.SlugName);
        File.WriteAllText(starterScript, GenerateStarterScript(packOptions.SlugName, packOptions.Name), Encoding.UTF8);
        MakeExecutable(starterScript);

        string desktopFilePath = Path.Combine(applicationsFolder, $"{packOptions.SlugName}.desktop");
        File.WriteAllText(desktopFilePath, GenerateDesktopFile(packOptions.SlugName), Encoding.UTF8);

        var iconPath = Path.Combine(directory, "icon.svg");
        if (File.Exists(iconPath))
        {
            File.Copy(iconPath, Path.Combine(iconsFolder, $"{packOptions.SlugName}.svg"), true);
            File.Copy(iconPath, Path.Combine(pixmapsFolder, $"{packOptions.SlugName}.png"), true);
        }

        string debFilePath = Path.Combine(directory, $"{packOptions.SlugName}-{platform}.deb");
        
        var publish = RunCommand($"dpkg-deb --root-owner-group --build {stagingFolder} {debFilePath}");
        if (!publish.IsSuccess)
        {
            throw new Exception(publish.Content);
        }

        Console.WriteLine($"Deb package created: {debFilePath}");
    }

    private static string GenerateControlFile(IPackOptions packOptions)
    {
        return $"""
                 Maintainer: {packOptions.Email}
                 Description: {packOptions.Description}
                 Package: {packOptions.SlugName}
                 Version: {packOptions.Version}
                 Section: utils
                 Priority: optional
                 Architecture: {packOptions.Architecture}
                 Installed-Size: 50000
                 Depends: libx11-6, libice6, libsm6, libfontconfig1, ca-certificates, tzdata, libc6, libgcc1 | libgcc-s1, libstdc++6, zlib1g
                 Homepage: {packOptions.HomePage}
                 """ + Environment.NewLine;
    }

    private static string GenerateStarterScript(string normalizeName, string name)
    {
        return $$"""
                #!/bin/bash
                exec /usr/lib/{{normalizeName}}/{{name}} "$@"
                """;
    }

    private static string GenerateDesktopFile(string name)
    {
        return $"""
                [Desktop Entry]
                Name={name}
                Comment={name} application
                Icon={name}
                Exec={name}
                Terminal=false
                Type=Application
                Categories=Utility;
                """;
    }

    private static void MakeExecutable(string filePath)
    {
        RunCommand($"chmod +x {filePath}");
    }

    private static (bool IsSuccess, string Content) RunCommand(string command)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = $"-c \"{command}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var output = new StringBuilder();
        var errorOutput = new StringBuilder();

        using Process process = Process.Start(psi);
        // Чтение вывода стандартного потока и ошибок
        process.OutputDataReceived += (sender, args) =>
        {
            if (args.Data != null)
                output.AppendLine(args.Data);
        };

        process.ErrorDataReceived += (sender, args) =>
        {
            if (args.Data != null)
                errorOutput.AppendLine(args.Data);
        };

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();

        return (process.ExitCode == 0, $"Output:\n{output}\nError Output:\n{errorOutput}");
    }
}