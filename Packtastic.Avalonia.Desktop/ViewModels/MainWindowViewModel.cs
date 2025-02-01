using Packtastic.Avalonia.Desktop.ViewModels.Base;

namespace Packtastic.Avalonia.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public PageViewModelBase ProjectsViewModel => new PackWindowViewModel();
}