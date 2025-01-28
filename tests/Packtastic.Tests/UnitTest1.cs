using Packtastic.Avalonia;

namespace Packtastic.Tests;

public class Tests
{
    private PackManager _packtasktic;
    private PackJsonConfig _jsonConfig;

    [SetUp]
    public void Setup()
    {
        _jsonConfig = new PackJsonConfig(Environment.CurrentDirectory);
        _packtasktic = new PackManager(_jsonConfig);
    }

    [Test]
    public void AddProjectsDirectory()
    {
        var directory = "D:\\Projects\\RiderProjects";
        var packageDirectory = _packtasktic.Projects.AddProjectDirectoryAsync(directory);
        
        Assert.Pass();
    }

    [Test]
    public void CanAddProjectsDirectory()
    {
        var directory = "D:\\Projects\\RiderProjects";
        var packageDirectory = _packtasktic.Projects.AddProjectDirectoryAsync(directory);
        
        Assert.Pass();
    }

    [Test]
    public async Task GetSolutions()
    {
        var packageDirectory = await _packtasktic.Projects.GetSolutionsAsync();
        
        Assert.Pass();
    }

    [Test]
    public async Task GetProjects()
    {
        var packageDirectory = await _packtasktic.Projects.GetSolutionsAsync();
        
        Assert.Pass();
    }
}