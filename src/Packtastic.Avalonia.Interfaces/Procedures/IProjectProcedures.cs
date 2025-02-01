namespace Packtastic.Avalonia.Procedures;

public interface IProjectProcedures
{
    Task AddProjectDirectoryAsync(string directory, CancellationToken token = default);
    Task<IBuildProject> CreateBuildProjectAsync(IProject project, CancellationToken token = default);
    Task<IReadOnlyCollection<IBuildProject>> GetBuildProjectsAsync(CancellationToken token = default);
    Task<IReadOnlyCollection<ISolution>> GetSolutionsAsync();
    Task UpdateBuildProjectAsync(IBuildProject selectedBuildProject);
}