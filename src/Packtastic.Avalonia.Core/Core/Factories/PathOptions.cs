using Packtastic.Avalonia.Core.Helpers;

namespace Packtastic.Avalonia.Core.Factories;

public class PathOptions(string projectName, string path) : IPackOptions
{
    public string Name { get; set; } = projectName;
    public string SlugName { get; set; } = projectName.ToSlug();
    public string BinaryDirectory { get; set; } = path;
    public string Email { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string HomePage { get; set; } = string.Empty;
    public string Architecture { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;

    public void Validate()
    {
    }
}