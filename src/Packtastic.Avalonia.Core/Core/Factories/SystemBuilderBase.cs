using System.Diagnostics;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Logging;

namespace Packtastic.Avalonia.Core.Factories;

public abstract class SystemBuilderBase(string osPrefix, string platform) : IOperationSystemBuilder
{
    private string RuntimeIdentity => $"{osPrefix}{platform}";
    private string OutputDirectory => Path.Combine("bin", "Packtastic", RuntimeIdentity);

    public bool BuildProject(string projectPath, string configuration = "Release")
    {
        try
        {
            var globalProperties = new Dictionary<string, string>
            {
                { "Configuration", configuration },
                { "Platform", platform },
                { "RuntimeIdentifier", RuntimeIdentity },
                { "OutputPath", OutputDirectory }
            };

            var projectCollection = new ProjectCollection(globalProperties);
            var buildParameters = new BuildParameters(projectCollection)
            {
                Loggers = [new ConsoleLogger()]
            };

            var buildRequest = new BuildRequestData(
                projectPath,
                globalProperties!,
                null,
                ["Build"],
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