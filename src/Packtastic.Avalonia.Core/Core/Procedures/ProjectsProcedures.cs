using Packtastic.Avalonia.Models;
using Packtastic.Avalonia.Procedures;

namespace Packtastic.Avalonia.Core.Procedures;

internal class ProjectsProcedures(IPackStorage storage) : IProjectProcedures
{
    public Task AddProjectDirectoryAsync(string directory, CancellationToken token = default)
    {
        storage.Configuration.AddProjectsDirectory(directory);

        return storage.SaveConfigurationAsync(token);
    }

    public async Task<IReadOnlyCollection<ISolution>> GetSolutionsAsync()
    {
        var tasks = storage.Configuration.ProjectsDirectories
            .Select(async directory =>
            {
                var solutionFiles = await Task.Run(() => Directory.EnumerateFiles(directory, "*.sln", SearchOption.AllDirectories));
                return solutionFiles.Select(file => new SolutionSln(file));
            });

        var projects = await Task.WhenAll(tasks);

        return projects.SelectMany(project => project).ToArray();
    }
}