namespace Packtastic.Avalonia.Core.Factories;

public class MsiPackOptions : PackOptions, IExternalPackOptions
{
    public required string DisplayName { get; set; }
    public string? BackgroundImagePath { get; set; }
    public string? BannerImagePath { get; set; }
    public string? ShortCutFileName { get; set; }
    public string? LicenseRtfPath { get; set; }
}