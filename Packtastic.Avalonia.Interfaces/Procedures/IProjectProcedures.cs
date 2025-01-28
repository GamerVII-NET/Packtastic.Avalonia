namespace Packtastic.Avalonia.Procedures;

public interface IProjectProcedures
{
    Task AddProjectDirectoryAsync(string directory, CancellationToken token = default);
    Task<IReadOnlyCollection<ISolution>> GetSolutionsAsync();
}