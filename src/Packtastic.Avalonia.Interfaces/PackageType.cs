namespace Packtastic.Avalonia;

/// <summary>
/// Enum representing various package formats for software distribution.
/// </summary>
public enum PackageType
{
    /// <summary>Disk image format for macOS (DMG).</summary>
    Dmg,

    /// <summary>Debian package format (DEB) for Debian-based Linux distributions.</summary>
    Deb,

    /// <summary>Windows Installer package (MSI).</summary>
    Msi,

    /// <summary>Executable file for application installation on Windows (EXE).</summary>
    Exe,

    /// <summary>Universal binary file (Linux, Windows, macOS).</summary>
    Binary,

    /// <summary>Compressed archive format for simplified application packaging (ZIP).</summary>
    Zip,

    /// <summary>Compressed archive format for Unix-like systems (TAR.GZ).</summary>
    TarGz,

    /// <summary>Red Hat package format (RPM) for RHEL-based Linux distributions.</summary>
    Rpm,

    /// <summary>Portable application format for Linux (AppImage).</summary>
    AppImage,

    /// <summary>Containerized application format for Linux (Flatpak).</summary>
    Flatpak,

    /// <summary>Universal package format for Linux (Snap).</summary>
    Snap,

    /// <summary>Unknown or unsupported package format.</summary>
    Unknown
}