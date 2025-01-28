using System.Text.Json;
using Packtastic.Avalonia.Core.Extensions;

namespace Packtastic.Avalonia.Core.Helpers;

public class JsonStorage(PackJsonConfig config) : IPackStorage
{
    public IConfiguration Configuration { get; private set; } = new PackConfiguration();

    public Task SaveConfigurationAsync(CancellationToken token = default)
    {
        CreateConfigureFolder();

        return Configuration.SaveToFileSystemAsync(config.ConfigFile, token);
    }

    public async Task RestoreConfigurationAsync(CancellationToken token = default)
    {
        Configuration = await Configuration.GetFromFileAsync(config.ConfigFile, token);
    }

    private void CreateConfigureFolder()
    {
        if (!string.IsNullOrEmpty(config.Directory) && !Directory.Exists(config.Directory))
        {
            Directory.CreateDirectory(config.Directory);
        }
    }
}