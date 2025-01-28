using Microsoft.Build.Locator;
using Packtastic.Avalonia.Core.Helpers;
using Packtastic.Avalonia.Core.Procedures;
using Packtastic.Avalonia.Procedures;

namespace Packtastic.Avalonia;

public class PackManager
{
    private IPackStorage PackStorage { get; }
    public IProjectProcedures Projects { get; }

    public PackManager(PackJsonConfig jsonConfig)
    {
        PackStorage = new JsonStorage(jsonConfig);
        Projects = new ProjectsProcedures(PackStorage);

        PackStorage.RestoreConfigurationAsync().Wait();
    }

    public void Register()
    {
        var instances = MSBuildLocator.QueryVisualStudioInstances();
        var instance = instances.FirstOrDefault();
        
        if (!MSBuildLocator.IsRegistered)
        {
            MSBuildLocator.RegisterInstance(instance);
        }
    }
}