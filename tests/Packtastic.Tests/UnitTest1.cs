using System.Runtime.InteropServices;
using Microsoft.Build.Locator;
using Packtastic.Avalonia;
using Packtastic.Avalonia.Core.Factories;
using Packtastic.Avalonia.Core.Helpers;

namespace Packtastic.Tests;

public class Tests
{
    private PackManager _packtasktic;
    private PackJsonConfig _jsonConfig;
    private string _directory;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _jsonConfig = new PackJsonConfig(Environment.CurrentDirectory);
        _packtasktic = new PackManager(_jsonConfig);
        _packtasktic.Register();
        
        if (OperatingSystem.IsWindows())
        {
            _directory = "D:\\Projects\\RiderProjects";
        }
        
        if (OperatingSystem.IsLinux())
        {
            _directory = "/home/gamervii/RiderProjects";
        }
    }

    [Test]
    public async Task AddProjectsDirectory()
    {
        await _packtasktic.Projects.AddProjectDirectoryAsync(_directory);
        
        Assert.Pass();
    }

    [Test]
    public async Task CanAddProjectsDirectory()
    {
        await _packtasktic.Projects.AddProjectDirectoryAsync(_directory);
        
        Assert.Pass();
    }

    [Test]
    public async Task GetSolutions()
    {
        var solutions = await _packtasktic.Projects.GetSolutionsAsync();
        
        Assert.That(solutions, Is.Not.Empty);
    }

    [Test]
    public async Task GetProjects()
    {
        var solutions = await _packtasktic.Projects.GetSolutionsAsync();
        
        var solution = solutions.Single(c => c.Name == "Gml.Launcher");

        var projects = await solution.GetProjectsAsync();
        
        Assert.That(projects, Is.Not.Empty);
    }

    [Test]
    public async Task GetAvaloniaProjects()
    {
        var solutions = await _packtasktic.Projects.GetSolutionsAsync();
        
        var solution = solutions.Single(c => c.Name == "Gml.Launcher");

        var projects = await solution.GetProjectsAsync();

        var avaloniaProject = projects.FirstOrDefault(c => c.AnyPackage("Avalonia.Desktop"));
        
        Assert.That(avaloniaProject, Is.Not.Null);
    }

    [Test]
    public async Task CreatePacktasticProject()
    {
        var solutions = await _packtasktic.Projects.GetSolutionsAsync();
        
        var solution = solutions.Single(c => c.Name == "Gml.Launcher");

        var projects = await solution.GetProjectsAsync();

        var avaloniaProject = projects.Single(c => c.AnyPackage("Avalonia.Desktop"));

        var buildProject = await _packtasktic.Projects.CreateBuildProjectAsync(avaloniaProject);
        
        Assert.That(avaloniaProject, Is.Not.Null);
    }

    [Test]
    public async Task GetPacktasticProjects()
    {
        var buildProjects = await _packtasktic.Projects.GetBuildProjectsAsync();
        
    }

    [Test]
    public async Task BuildPacktasticProjects()
    {
        var buildProjects = await _packtasktic.Projects.GetBuildProjectsAsync();
        var project = buildProjects.Single(c => c.Name == "Gml.Launcher");

        await project.BuildAllPlatformsAsync();
    }

    [Test]
    public async Task BuildPacktasticProject()
    {
        var buildProjects = await _packtasktic.Projects.GetBuildProjectsAsync();
        var project = buildProjects.Single(c => c.Name == "Gml.Launcher");

        await project.BuildPlatformsAsync(RuntimeInformation.RuntimeIdentifier);
    }

    [Test]
    public async Task PackPacktasticProject()
    {
        var buildProjects = await _packtasktic.Projects.GetBuildProjectsAsync();
        var project = buildProjects.Single(c => c.Name == "Gml.Launcher");

        var packOptions = new PackOptions
        {
            Name = project.Name,
            SlugName = project.Name.ToSlug(),
            BinaryDirectory = Path.GetDirectoryName(project.AbsolutePath)!,
            Email = "orders@recloud.tech",
            Description = "Игровой проект Minecraft",
            Version = "1.0.0.0",
            HomePage = "https://github.com/Gml-Launcher/Gml.Launcher"
        };
        
        await project.PackPlatformsAsync(PackageType.Deb, RuntimeInformation.RuntimeIdentifier, packOptions);
    }

    [Test]
    public async Task PackZipPacktasticProject()
    {
        var buildProjects = await _packtasktic.Projects.GetBuildProjectsAsync();
        var project = buildProjects.Single(c => c.Name == "Gml.Launcher");
        
        await project.PackPlatformsAsync(PackageType.Zip, RuntimeInformation.RuntimeIdentifier, new PathOptions(project.Name, Path.GetDirectoryName(project.AbsolutePath)!));
    }
}