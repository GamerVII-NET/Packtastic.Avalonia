using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Packtastic.Avalonia.Desktop.ViewModels;

namespace Packtastic.Avalonia.Desktop.Views;

public partial class ProjectPage : ReactiveUserControl<PackWindowViewModel>
{
    public ProjectPage()
    {
        InitializeComponent();
    }

    private async void OpenDirectoryFolder(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        if (topLevel is null)
        {
            return;
        }

        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select a folder4"
        });

        if (ViewModel is not null && folders.Any())
        {
            await ViewModel.AddSolutionPaths(folders.Select(c => c.Path.AbsolutePath));
        }

    }
}