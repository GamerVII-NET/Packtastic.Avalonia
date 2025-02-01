using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using DynamicData.Binding;
using Packtastic.Avalonia.Desktop.ViewModels.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Packtastic.Avalonia.Desktop.ViewModels;

public class PackWindowViewModel : PageViewModelBase
{
    private readonly PackManager _packManager;
    private IReadOnlyCollection<ISolution> _solutions;
    [Reactive] public ObservableCollectionExtended<ISolution> Solutions { get; set; }
    [Reactive] public bool SolutionsIsEmpty { get; set; }
    
    public PackWindowViewModel(PackManager? packManager = null)
    {
        _packManager = packManager 
                       ?? Locator.Current.GetService<PackManager>() 
                       ?? throw new Exception("No pack manager found");

        RxApp.MainThreadScheduler.Schedule(LoadData);

    }

    private void LoadData()
    {
        ExecuteFromNewThread(async () =>
        {
            _solutions = await _packManager.Projects.GetSolutionsAsync();

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Solutions = new ObservableCollectionExtended<ISolution>(_solutions);
                SolutionsIsEmpty = Solutions.Count == 0;
            });
        });
    }

    public async Task AddSolutionPaths(params IEnumerable<string> paths)
    {
        foreach (var path in paths)
        {
            await _packManager.Projects.AddProjectDirectoryAsync(path);
        }
        
        LoadData();
    }
}