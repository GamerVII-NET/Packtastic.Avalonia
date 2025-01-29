# Packtastic.Avalonia

## Project Description

This project is a cross-platform tool for building and publishing applications based on **Avalonia**.
The main goal of this project is to provide convenient packaging of applications into various formats
for different operating systems.

| Package Type | Description                                               | Supported OS               | Supported |
|--------------|-----------------------------------------------------------|----------------------------|-----------|
| **dmg**      | Packaging applications into disk image formats.           | macOS                      | ✘         |
| **deb**      | Format for creating Debian packages.                      | Linux (Debian-based)       | ✔         |
| **msi**      | Application installation using Windows Installer.         | Windows                    | ✘         |
| **exe**      | Executable files for application installation.            | Windows                    | ✔         |
| **binary**   | Universal binary files.                                   | Linux, Windows, macOS      | ✔         |
| **zip**      | Compressed archive for simplified application packaging.  | Windows, macOS, Linux      | ✘         |
| **tar.gz**   | Compressed archive for distribution in Unix-like systems. | Linux, macOS               | ✘         |
| **rpm**      | Red Hat package format for RPM-based Linux distributions. | Linux (RHEL, Fedora, etc.) | ✘         |
| **AppImage** | Portable application format for Linux.                    | Linux                      | ✘         |
| **flatpak**  | Containerized format for applications.                    | Linux                      | ✘         |
| **snap**     | Universal package format for Linux.                       | Linux                      | ✘         |

Additionally, the project includes its own graphical application, providing a convenient user interface that allows
users to:

- Configure the build process.
- Specify publishing parameters.
- Integrate with various publishing platforms.

## Key Features

- **Cross-platform support**: The application is supported on Windows, Linux, and macOS.
- **Avalonia support**: Specific settings and optimizations for Avalonia-based applications.
- **User-friendly graphical interface**: Managing build and publishing processes through a UI.
- **Flexible configuration**: Ability to set parameters for different package types and operating systems.

## Usage

### Installation

The installation instructions will be added based on the release format (e.g., available through NuGet,
App Store, Snap, etc.).

### Running

To launch the application:

1. Install the project on your operating system.
2. Run the executable file and follow the instructions in the graphical interface.

## Compatibility

This project is being developed with support for the following platforms:

- **Windows** (x64)
- **Linux** (major distributions)
- **macOS** (ARM64 and x64)

## Future Plans

- Adding support for packaging in **AppImage**.
- Automated deployment to platforms like GitHub Releases, Azure DevOps, etc.
- Enhancing the functionality of the user interface.

## Authors

The project is being developed by a team interested in streamlining and standardizing the process of packaging
cross-platform applications.

## Contributing

Your contributions are welcome! If you have ideas or suggestions, feel free to open an issue or create a pull request.