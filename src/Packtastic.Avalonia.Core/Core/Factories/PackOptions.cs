namespace Packtastic.Avalonia.Core.Factories;

public class PackOptions : IPackOptions
{
    public required string Name { get; set; }
    public required string SlugName { get; set; }
    public required string BinaryDirectory { get; set; }
    public required string Email { get; set; }
    public required string Description { get; set; }
    public required string Version { get; set; }
    public required string HomePage { get; set; }
    public void Validate()
    {
        if (string.IsNullOrEmpty(Name))
            throw new ArgumentException(nameof(Name));
        if (string.IsNullOrEmpty(Version))
            throw new ArgumentException(nameof(Version));
        if (string.IsNullOrEmpty(HomePage))
            throw new ArgumentException(nameof(HomePage));
        if (string.IsNullOrEmpty(Description))
            throw new ArgumentException(nameof(Description));
        if (string.IsNullOrEmpty(Email))
            throw new ArgumentException(nameof(Email));
    }
}