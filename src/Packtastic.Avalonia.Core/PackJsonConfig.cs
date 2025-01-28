namespace Packtastic.Avalonia;

public class PackJsonConfig(string directory)
{
    public string Directory { get; set; } = directory;
    public string ConfigFile = Path.Combine(directory, "config.json");
}