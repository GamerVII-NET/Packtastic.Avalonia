using System.Collections.Concurrent;

namespace Packtastic.Avalonia.Core.Helpers;

public class PackConfiguration : IConfiguration
{
    public ICollection<string> ProjectsDirectories { get; set; } = new HashSet<string>();
    public ConcurrentDictionary<string, IBuildProject> BuildProjects { get; set; } = new();

    public Task SaveConfigurationAsync(CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public void AddProjectsDirectory(string directory)
    {
        if (string.IsNullOrWhiteSpace(directory) ||
            ProjectsDirectories.Any(c => c.Equals(directory, StringComparison.OrdinalIgnoreCase)))
            return;

        ProjectsDirectories.Add(directory);
    }

    public void AddBuildProject(IBuildProject project)
    {
        BuildProjects.TryAdd(project.Name, project);
    }

    public void UpdateBuildProject(IBuildProject selectedBuildProject)
    {
        if (BuildProjects.ContainsKey(selectedBuildProject.Name))
        {
            BuildProjects[selectedBuildProject.Name] = selectedBuildProject;
        }
        else
        {
            throw new KeyNotFoundException(
                $"The build project with name '{selectedBuildProject.Name}' does not exist.");
        }
    }
}