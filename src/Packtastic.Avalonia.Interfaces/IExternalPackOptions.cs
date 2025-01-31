namespace Packtastic.Avalonia;

public interface IExternalPackOptions : IPackOptions
{
    string DisplayName { get; set; }
    string? BackgroundImagePath { get; set; }
    /// <summary>
    /// Gets or sets the path to a background image. The image must have a size of 493 x 58.
    /// </summary>
    string? BannerImagePath { get; set; }

    string? ShortCutFileName { get; set; }
    string? LicenseRtfPath { get; set; }
}