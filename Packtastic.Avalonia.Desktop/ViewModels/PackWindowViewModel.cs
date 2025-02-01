using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using DynamicData.Binding;
using Microsoft.Build.Exceptions;
using Packtastic.Avalonia.Desktop.ViewModels.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Packtastic.Avalonia.Desktop.ViewModels;

public class PackWindowViewModel : PageViewModelBase
{
    private readonly PackManager _packManager;
    private IReadOnlyCollection<ISolution> _solutions;
    private IReadOnlyCollection<IProject> _projects;
    private IReadOnlyCollection<IBuildProject> _buildProjects;
    [Reactive] public ObservableCollectionExtended<ISolution> Solutions { get; set; }
    [Reactive] public ObservableCollectionExtended<IProject> SolutionProjects { get; set; }
    [Reactive] public ObservableCollectionExtended<IBuildProject> BuildProjects { get; set; }
    [Reactive] public ISolution? SelectedSolution { get; set; }
    [Reactive] public IProject? SelectedProject { get; set; }
    [Reactive] public bool SolutionsIsEmpty { get; set; }
    [Reactive] public bool ProjectsIsEmpty { get; set; }
    [Reactive] public bool BuildProjectsIsEmpty { get; set; }
    public ReactiveCommand<Unit, Unit> CreateProjectCommand { get; }

    public PackWindowViewModel(PackManager packManager)
    {
        _packManager = packManager;
        CreateProjectCommand = ReactiveCommand.CreateFromTask(OnCreateProject);

        RxApp.MainThreadScheduler.Schedule(LoadData);

        this.WhenAnyValue(vm => vm.SelectedSolution)
            .Where(selectedSolution => selectedSolution != null)
            .Subscribe(LoadProjectsForSelectedSolution!);
        
        this.WhenAnyValue(vm => vm.SelectedProject)
            .Where(selectedSolution => selectedSolution != null)
            .Subscribe(LoadProjectsForSelectedProject!);
    }

    private async Task OnCreateProject()
    {
        if (SelectedProject == null)
        {
            return;
        }

        var buildProject = await _packManager.Projects.CreateBuildProjectAsync(SelectedProject);
        
    }

    private void LoadData()
    {
        IsProcessing = true;
        ExecuteFromNewThread(async () =>
        {
            try
            {
                _solutions = await _packManager.Projects.GetSolutionsAsync();

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Solutions = new ObservableCollectionExtended<ISolution>(_solutions.OrderBy(c => c.Name));
                    SolutionsIsEmpty = Solutions.Count == 0;
                });
            }
            finally
            {
                IsProcessing = false;
            }
        });
    }

    private void LoadProjectsForSelectedSolution(ISolution selectedSolution)
    {
        IsProcessing = true;
        ExecuteFromNewThread(async () =>
        {
            try
            {
                _projects = await selectedSolution.GetProjectsAsync();

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    SolutionProjects = new ObservableCollectionExtended<IProject>(_projects.OrderBy(c => c.Name));
                    ProjectsIsEmpty = SolutionProjects.Count == 0;
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
            finally
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    IsProcessing = false;
                });
            }
        });
    }

    private void LoadProjectsForSelectedProject(IProject selectedSolution)
    {
        IsProcessing = true;
        ExecuteFromNewThread(async () =>
        {
            try
            {
                if (SelectedProject is not null)
                {
                    _buildProjects = await _packManager.Projects.GetBuildProjectsAsync();

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        BuildProjects = new ObservableCollectionExtended<IBuildProject>(_buildProjects.Where(c => c .AbsolutePath == SelectedProject.AbsolutePath).OrderBy(c => c.Name));
                        BuildProjectsIsEmpty = BuildProjects.Count == 0;
                    });
                    
                }
            }
            catch (InvalidProjectFileException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    IsProcessing = false;
                });
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