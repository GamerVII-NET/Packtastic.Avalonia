using System.Collections.Concurrent;

namespace Packtastic.Avalonia.Core.Helpers;

public class PackConfiguration : IConfiguration
{
    public ICollection<string> ProjectsDirectories { get; set; } = new HashSet<string>();
    
    public Task SaveConfigurationAsync(CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public void AddProjectsDirectory(string directory)
    {
        ProjectsDirectories.Add(directory);
    }
}