using System.Diagnostics;
using System.Text.RegularExpressions;
using Packtastic.Avalonia.Core.Helpers;

namespace Packtastic.Avalonia.Core.Factories;

public class MsiPacker(string platform) : IOperationSystemPacker
{
    public void Pack(IPackOptions packOptions)
    {
        if (OperatingSystem.IsWindows())
        {
            throw new InvalidOperationException("Windows MSI packer is not implemented yet.");
        }
        
        CheckWix();

        var directory = Path.Combine(packOptions.BinaryDirectory, "bin", "Packtastic", platform);

        if (!Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException($"The directory '{directory}' does not exist.");
        }
        
        var template = "Templates/Wix/template.wxs";
        var fileTemplate = "Templates/Wix/file-component.xml";

        if (!File.Exists(fileTemplate))
        {
            throw new FileNotFoundException($"The file '{fileTemplate}' does not exist.");
        }
        
        if (!File.Exists(template))
        {
            throw new FileNotFoundException($"The file '{template}' does not exist.");
        }
        
        var fileTemplateContent = File.ReadAllText(fileTemplate);
        
        var filesContent = string.Empty;

        foreach (var file in new DirectoryInfo(directory).GetFiles("*", SearchOption.AllDirectories))
        {
            filesContent += fileTemplateContent
                .Replace(@"{{id}}", file.Name.ToSlug().Replace("-", string.Empty))
                .Replace(@"{{name}}", file.Name)
                .Replace(@"{{path}}", file.FullName);
        }
        
        var content = File.ReadAllText(template)
            .Replace( @"{{name}}", packOptions.Name)
            .Replace("{{version}}", packOptions.Version)
            .Replace("{{publisher}}", packOptions.CompanyName)
            .Replace("{{files}}", filesContent)
            .Replace("{{guid}}", Guid.NewGuid().ToString());
        
        var tempWix = Path.Combine(Path.GetTempPath(), $"{packOptions.SlugName}-{platform}.wxs");
        File.WriteAllText(tempWix, content);
        // System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("candle", tempWix) { UseShellExecute = true })!.WaitForExit();
        Process.Start(new System.Diagnostics.ProcessStartInfo("wix", $"build {tempWix}") { UseShellExecute = true })!.WaitForExit();

        
        
        // System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("candle", tempWix) { UseShellExecute = true })!.WaitForExit();
        // System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("light", $"{packOptions.SlugName}-{platform}.wixobj") { UseShellExecute = true })!.WaitForExit();
        
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