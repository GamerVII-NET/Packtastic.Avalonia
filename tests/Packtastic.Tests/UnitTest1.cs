using Microsoft.Build.Locator;
using Packtastic.Avalonia;

namespace Packtastic.Tests;

public class Tests
{
    private PackManager _packtasktic;
    private PackJsonConfig _jsonConfig;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _jsonConfig = new PackJsonConfig(Environment.CurrentDirectory);
        _packtasktic = new PackManager(_jsonConfig);
        _packtasktic.Register();
    }

    [Test]
    public async Task AddProjectsDirectory()
    {
        var directory = "D:\\Projects\\RiderProjects"; 
        await _packtasktic.Projects.AddProjectDirectoryAsync(directory);
        
        Assert.Pass();
    }

    [Test]
    public async Task CanAddProjectsDirectory()
    {
        var directory = "D:\\Projects\\RiderProjects";
        await _packtasktic.Projects.AddProjectDirectoryAsync(directory);
        
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
    public async Task BuildPacktasticProject()
    {
        var buildProjects = await _packtasktic.Projects.GetBuildProjectsAsync();
        var project = buildProjects.Single(c => c.Name == "Gml.Launcher");

        await project.BuildAllPlatformsAsync();

    }
}