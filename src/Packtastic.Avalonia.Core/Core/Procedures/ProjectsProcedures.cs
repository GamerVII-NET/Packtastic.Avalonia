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

    public Task<IReadOnlyCollection<IBuildProject>> GetBuildProjectsAsync(CancellationToken token = default)
    {
        var projects = storage.Configuration.BuildProjects.Values.ToArray();
        
        return Task.FromResult<IReadOnlyCollection<IBuildProject>>(projects);
    }

    public async Task<IReadOnlyCollection<ISolution>> GetSolutionsAsync()
    {
        var tasks = storage.Configuration.ProjectsDirectories
            .Select(async directory =>
            {
                var solutionFiles = await Task.Factory.StartNew(() => Directory.EnumerateFiles(directory, "*.sln", SearchOption.AllDirectories), TaskCreationOptions.LongRunning);
                return solutionFiles.Select(file => new SolutionSln(file));
            });

        var projects = await Task.WhenAll(tasks);

        return projects.SelectMany(project => project).ToArray();
    }

    public Task UpdateBuildProjectAsync(IBuildProject selectedBuildProject)
    {
        if (storage.Configuration.BuildProjects.ContainsKey(selectedBuildProject.Name))
        {
            storage.Configuration.UpdateBuildProject(selectedBuildProject);
        }

        return storage.SaveConfigurationAsync();
    }

    public async Task<IBuildProject> CreateBuildProjectAsync(IProject project, CancellationToken token = default)
    {
        var buildProject = new BuildProject(project);

        storage.Configuration.AddBuildProject(buildProject);
        
        await storage.SaveConfigurationAsync(token);
        
        return buildProject;
    }
}