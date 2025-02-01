using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using DynamicData.Binding;
using Microsoft.Build.Exceptions;
using Packtastic.Avalonia.Desktop.ViewModels.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Packtastic.Avalonia.Desktop.ViewModels;

public class PackWindowViewModel : PageViewModelBase
{
    private readonly PackManager _packManager;
    private IReadOnlyCollection<ISolution> _solutions;
    private IReadOnlyCollection<IProject> _projects;
    [Reactive] public ObservableCollectionExtended<ISolution> Solutions { get; set; }
    [Reactive] public ObservableCollectionExtended<IProject> SolutionProjects { get; set; }
    [Reactive] public ISolution? SelectedSolution { get; set; }
    [Reactive] public IProject? SelectedProject { get; set; }
    [Reactive] public bool SolutionsIsEmpty { get; set; }
    [Reactive] public bool ProjectsIsEmpty { get; set; }
    
    public PackWindowViewModel(PackManager? packManager = null)
    {
        _packManager = packManager 
                       ?? Locator.Current.GetService<PackManager>() 
                       ?? throw new Exception("No pack manager found");
        
        

        RxApp.MainThreadScheduler.Schedule(LoadData);
        
        this.WhenAnyValue(vm => vm.SelectedSolution)
            .Where(selectedSolution => selectedSolution != null)
            .Subscribe(LoadProjectsForSelectedSolution!);

    }

    private void LoadData()
    {
        ExecuteFromNewThread(async () =>
        {
            _solutions = await _packManager.Projects.GetSolutionsAsync();
    
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Solutions = new ObservableCollectionExtended<ISolution>(_solutions.OrderBy(c => c.Name));
                SolutionsIsEmpty = Solutions.Count == 0;
            });
        });
    }
    
    private void LoadProjectsForSelectedSolution(ISolution selectedSolution)
    {
        ExecuteFromNewThread(async () =>
        {
            try
            {
                _projects = await selectedSolution.GetProjectsAsync();
    
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    SolutionProjects = new ObservableCollectionExtended<IProject>(_projects.OrderBy(c => c.Name));
                    ProjectsIsEmpty = Solutions.Count == 0;
                });
            }
            catch (InvalidProjectFileException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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