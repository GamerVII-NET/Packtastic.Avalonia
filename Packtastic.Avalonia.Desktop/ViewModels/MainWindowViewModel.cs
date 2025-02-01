using System;
using Packtastic.Avalonia.Desktop.ViewModels.Base;
using Splat;

namespace Packtastic.Avalonia.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public PageViewModelBase ProjectsViewModel { get; }
    public PageViewModelBase BuildPageViewModel { get; }

    public MainWindowViewModel()
    {
        var packManager = Locator.Current.GetService<PackManager>()
                          ?? throw new Exception("No pack manager found");

        ProjectsViewModel = new PackWindowViewModel(packManager);
        BuildPageViewModel = new BuildPageViewModel(packManager);
    }
}