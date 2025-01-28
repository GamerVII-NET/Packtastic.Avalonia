using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace Packtastic.Avalonia.Models;

public class Project(string projectName, ProjectInSolution project) : IProject
{
    private Microsoft.Build.Evaluation.Project _msbuildProject = new(project.AbsolutePath);
    public string AbsolutePath { get; set; } = project.AbsolutePath;
    private ProjectInSolution _project { get; } = project;
    public string Name { get; } = projectName;

    public bool AnyPackage(string packageName)
    {
        return _msbuildProject.Items
            .Any(item =>
                item.ItemType == "PackageReference" && 
                item.EvaluatedInclude == packageName);
    }
}