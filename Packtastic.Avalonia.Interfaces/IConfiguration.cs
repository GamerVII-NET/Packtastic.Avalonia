namespace Packtastic.Avalonia;

public interface IConfiguration
{
    public ICollection<string> ProjectsDirectories { get; set; }
    void AddProjectsDirectory(string directory);
}