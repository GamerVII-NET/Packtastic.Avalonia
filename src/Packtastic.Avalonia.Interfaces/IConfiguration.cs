using System.Collections.Concurrent;

namespace Packtastic.Avalonia;

public interface IConfiguration
{
    public ICollection<string> ProjectsDirectories { get; set; }
    public ConcurrentDictionary<string, IBuildProject> BuildProjects { get; set; }
    void AddProjectsDirectory(string directory);
    void AddBuildProject(IBuildProject project);
    void UpdateBuildProject(IBuildProject selectedBuildProject);
}