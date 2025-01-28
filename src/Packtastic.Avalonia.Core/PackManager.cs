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
}