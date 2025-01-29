using System.Runtime.InteropServices;
using Microsoft.Build.Locator;
using Packtastic.Avalonia;

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

        project.Version = "1.0.0.0";
        project.HomePage = "https://github.com/Gml-Launcher/Gml.Launcher";
        project.Description = "Игровой проект Minecraft";
        project.Email = "orders@recloud.tech";
        
        await project.PackPlatformsAsync(PackageType.Deb, RuntimeInformation.RuntimeIdentifier);
    }
}