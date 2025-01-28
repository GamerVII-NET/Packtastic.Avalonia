using Microsoft.Build.Construction;

namespace Packtastic.Avalonia.Models;

public class SolutionSln(string filePath) : ISolution
{
    private SolutionFile _solutionFile { get; } = SolutionFile.Parse(filePath);
    public string Name { get; } = Path.GetFileNameWithoutExtension(filePath);
    public async Task<IReadOnlyCollection<IProject>> GetProjectsAsync()
    {
        var projects = _solutionFile.ProjectsInOrder
            .Where(p => p.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat)
            .Select(p => new Project(p.ProjectName, p))
            .ToList();

        return await Task.FromResult<IReadOnlyCollection<IProject>>(projects);
    }

    public override string ToString()
    {
        return $"Solution: {Name}";
    }
}