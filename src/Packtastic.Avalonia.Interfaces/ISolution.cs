namespace Packtastic.Avalonia;

public interface ISolution
{
    string Name { get; }
    Task<IReadOnlyCollection<IProject>> GetProjectsAsync();
}