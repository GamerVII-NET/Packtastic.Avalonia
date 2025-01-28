using Microsoft.Build.Construction;
using Packtastic.Avalonia.Core.Factories;

namespace Packtastic.Avalonia.Models;

public class BuildProject : IBuildProject
{
    public IProject? Project { get; set; }
    public string Name { get; set; }
    public string AbsolutePath { get; set; }
    private OperationSystemBuilderFactory _builderFactory = new();

    public BuildProject()
    {
    }

    public BuildProject(IProject project)
    {
        Project = project;
        Name = project.Name;
        AbsolutePath = project.AbsolutePath;
    }
    public bool AnyPackage(string packageName) => Project.AnyPackage(packageName);
    public Task BuildAllPlatformsAsync()
    {
        foreach (var platform in _builderFactory.OperationSystemBuilders.Keys)
        {
            _builderFactory.OperationSystemBuilders[platform].BuildProject(AbsolutePath);
        }
        
        return Task.CompletedTask;
    }
}