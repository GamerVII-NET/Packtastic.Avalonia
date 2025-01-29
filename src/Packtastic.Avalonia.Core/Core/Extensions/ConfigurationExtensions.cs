using System.Text.Json;
using Packtastic.Avalonia.Core.Helpers;
using Packtastic.Avalonia.Models;

namespace Packtastic.Avalonia.Core.Extensions;

public static class ConfigurationExtensions
{
    public static async Task SaveToFileSystemAsync(
        this IConfiguration configuration, 
        string filePath,
        CancellationToken token = default)
    {
        await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        await JsonSerializer.SerializeAsync(fileStream, configuration, new JsonSerializerOptions
        {
            WriteIndented = true
        }, token);
    }
    
    public static async Task<IConfiguration> GetFromFileAsync(
        this IConfiguration configuration, 
        string filePath,
        CancellationToken token = default)
    {
        if (!File.Exists(filePath))
        {
            return new PackConfiguration();
        }
        
        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var restoredConfig = await JsonSerializer.DeserializeAsync<PackConfiguration>(fileStream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new InterfaceConverter<IBuildProject, BuildProject>()
            }
        }, token);
        
        if (restoredConfig == null)
        {
            throw new InvalidOperationException("Failed to deserialize the configuration file.");
        }

        return restoredConfig;
    }
}