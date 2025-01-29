using System.Text.RegularExpressions;
using Microsoft.Build.Construction;
using Packtastic.Avalonia.Core.Factories;
using Packtastic.Avalonia.Core.Helpers;

namespace Packtastic.Avalonia.Models;

public class BuildProject : IBuildProject
{
    public IProject? Project { get; set; }
    public string Name { get; set; }
    public string Version { get; set; }
    public string HomePage { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string AbsolutePath { get; set; }
    private OperationSystemBuilderFactory _builderFactory = new();
    private OperationSystemPackerFactory _packerFactory = new();

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

    public Task BuildPlatformsAsync(string platform)
    {
        if (_builderFactory.OperationSystemBuilders.TryGetValue(platform, out var builder))
        {
            builder.BuildProject(AbsolutePath);
            return Task.CompletedTask;
        }

        throw new NotSupportedException();
    }

    public Task PackPlatformsAsync(PackageType packageType, string platform, IPackOptions packOptions)
    {
        if (!_packerFactory.OperationSystemPackers.TryGetValue(packageType, out var packers))
            throw new NotSupportedException();

        if (!packers.TryGetValue(platform, out var packer))
            throw new NotSupportedException();

        packOptions.Validate();

        packer.Pack(packOptions);

        return Task.CompletedTask;
    }
}