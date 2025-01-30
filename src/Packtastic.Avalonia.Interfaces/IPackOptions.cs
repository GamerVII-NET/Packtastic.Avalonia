namespace Packtastic.Avalonia;

public interface IPackOptions
{
    string Name { get; set; }
    string SlugName { get; set; }
    string BinaryDirectory { get; set; }
    string Email { get; set; }
    string Description { get; set; }
    string Version { get; set; }
    string HomePage { get; set; }
    string Architecture { get; set; }
    string CompanyName { get; set; }
    void Validate();
}