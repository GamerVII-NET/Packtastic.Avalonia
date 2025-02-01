using System;
using Avalonia;
using Avalonia.Input;
using Splat;

namespace Packtastic.Avalonia.Desktop.Core;

public static class AppBuilderDesktopExtensions
{
    public static AppBuilder RegisterDependencies(this AppBuilder builder)
    {
        Locator.CurrentMutable.Register(() => new PackJsonConfig(Environment.CurrentDirectory));
        Locator.CurrentMutable.Register(() => new PackManager(Locator.Current.GetService<PackJsonConfig>()!));
        
        
        return builder;
    }
}