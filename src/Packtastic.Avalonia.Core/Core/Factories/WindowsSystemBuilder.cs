using System.Diagnostics;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Logging;

namespace Packtastic.Avalonia.Core.Factories;

public class WindowsSystemBuilder : IOperationSystemBuilder
{
    private readonly string _platform;

    public WindowsSystemBuilder(string platform)
    {
        _platform = platform;
    }

    public bool Build(string solutionPath, string configuration = "Release")
    {
        try
        {
            var globalProperties = new Dictionary<string, string>
            {
                { "Configuration", configuration },
                { "Platform", _platform }
            };

            var projectCollection = new ProjectCollection(globalProperties);
            var buildParameters = new BuildParameters(projectCollection)
            {
                Loggers = new[] { new ConsoleLogger() }
            };

            var buildRequest = new BuildRequestData(
                solutionPath,
                globalProperties,
                null,
                new[] { "Build" },
                null
            );

            var buildManager = BuildManager.DefaultBuildManager;
            var buildResult = buildManager.Build(buildParameters, buildRequest);

            return buildResult.OverallResult == BuildResultCode.Success;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Сборка завершилась неудачей: {ex.Message}");
            return false;
        }
    }
    
    public bool BuildProject(string projectPath, string configuration = "Release")
    {
        try
        {
            var globalProperties = new Dictionary<string, string>
            {
                { "Configuration", configuration },
                { "Platform", _platform }
            };

            var projectCollection = new ProjectCollection(globalProperties);
            var buildParameters = new BuildParameters(projectCollection)
            {
                Loggers = [new ConsoleLogger()]
            };

            var buildRequest = new BuildRequestData(
                projectPath,
                globalProperties,
                null,
                new[] { "Build" },
                null
            );

            var buildManager = BuildManager.DefaultBuildManager;
            var buildResult = buildManager.Build(buildParameters, buildRequest);

            return buildResult.OverallResult == BuildResultCode.Success;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Сборка проекта завершилась неудачей: {ex.Message}");
            return false;
        }
    }
}