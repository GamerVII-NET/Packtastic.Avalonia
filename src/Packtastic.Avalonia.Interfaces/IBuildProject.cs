namespace Packtastic.Avalonia;

public interface IBuildProject : IProject
{
    string Name { get; set; }
    public string DisplayName { get; set; }
    public string SlugName { get; }
    string Version { get; set; }
    string HomePage { get; set; }
    string Description { get; set; }
    string Email { get; set; }
    Task<bool> BuildAllPlatformsAsync();
    Task<bool> BuildPlatformsAsync(string platform);
    Task PackPlatformsAsync(PackageType packageType, string platform, IPackOptions packOptions);
}