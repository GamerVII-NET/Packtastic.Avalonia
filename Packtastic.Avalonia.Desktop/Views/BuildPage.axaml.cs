using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Packtastic.Avalonia.Desktop.ViewModels;

namespace Packtastic.Avalonia.Desktop.Views;

public partial class BuildPage : ReactiveUserControl<BuildPageViewModel>
{
    public BuildPage()
    {
        InitializeComponent();
    }
}