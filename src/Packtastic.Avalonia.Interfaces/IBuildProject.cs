namespace Packtastic.Avalonia;

public interface IBuildProject : IProject
{
    string Name { get; set; }
    string Version { get; set; }
    string HomePage { get; set; }
    string Description { get; set; }
    string Email { get; set; }
    Task BuildAllPlatformsAsync();
    Task BuildPlatformsAsync(string platform);
    Task PackPlatformsAsync(PackageType packageType, string platform);
}