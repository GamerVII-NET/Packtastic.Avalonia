using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Avalonia.Threading;
using DynamicData.Binding;
using Microsoft.Build.Exceptions;
using Packtastic.Avalonia.Core.Factories;
using Packtastic.Avalonia.Core.Helpers;
using Packtastic.Avalonia.Desktop.ViewModels.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Packtastic.Avalonia.Desktop.ViewModels;

public class BuildPageViewModel : PageViewModelBase
{
    private readonly PackManager _packManager;
    private IReadOnlyCollection<IBuildProject> _buildProjects;

    [Reactive] public ObservableCollectionExtended<IBuildProject> BuildProjects { get; set; } = [];
    [Reactive] public IBuildProject? SelectedBuildProject { get; set; }
    [Reactive] public bool BuildProjectsIsEmpty { get; set; }
    [Reactive] public bool IsMsiBuildEnabled { get; set; }
    [Reactive] public bool IsDebBuildEnabled { get; set; }
    [Reactive] public string DisplayName { get; set; }
    [Reactive] public string SlugName { get; set; }
    [Reactive] public bool HasAllBuilds { get; set; }
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> BuildCommand { get; }
    public ReactiveCommand<Unit, Unit> PackCommand { get; }

    public BuildPageViewModel(PackManager packManager)
    {
        _packManager = packManager;

        SaveCommand = ReactiveCommand.CreateFromTask(OnSaveProject);
        BuildCommand = ReactiveCommand.CreateFromTask(OnBuildProject);
        PackCommand = ReactiveCommand.CreateFromTask(OnPackProject);

        this.WhenAnyValue(vm => vm.DisplayName)
            .Subscribe(name => SlugName = name.ToSlug());

        this.WhenAnyValue(vm => vm.SelectedBuildProject)
            .WhereNotNull()
            .Subscribe(name => { DisplayName = name.DisplayName; });

        RxApp.MainThreadScheduler.Schedule(LoadData);
    }

    private Task OnPackProject()
    {
        if (SelectedBuildProject == null)
            return Task.CompletedTask;
        
        IsProcessing = true;
        
        return ExecuteFromNewThread(async () =>
        {
            try
            {
                if (IsMsiBuildEnabled)
                {
                    await SelectedBuildProject.PackPlatformsAsync(PackageType.Msi, new MsiPackOptions
                    {
                        DisplayName = SelectedBuildProject.DisplayName,
                        Name = SelectedBuildProject.Name,
                        SlugName = SelectedBuildProject.SlugName,
                        CompanyName = SelectedBuildProject.CompanyName,
                        BinaryDirectory = Path.GetDirectoryName(SelectedBuildProject.AbsolutePath)!,
                        Email = SelectedBuildProject.Email,
                        Description = SelectedBuildProject.Description,
                        Version = SelectedBuildProject.Version,
                        HomePage = SelectedBuildProject.HomePage,
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
                await Dispatcher.UIThread.InvokeAsync(() => { IsProcessing = false; });
            }
        });
    }

    private Task OnBuildProject()
    {
        if (SelectedBuildProject == null)
            return Task.CompletedTask;

        IsProcessing = true;

        return ExecuteFromNewThread(async () =>
        {
            try
            {
                var hasAllBuilds = await SelectedBuildProject.BuildAllPlatformsAsync();
                await Dispatcher.UIThread.InvokeAsync(() => { HasAllBuilds = hasAllBuilds; });
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
                await Dispatcher.UIThread.InvokeAsync(() => { IsProcessing = false; });
            }
        });
    }

    private async Task OnSaveProject()
    {
        if (SelectedBuildProject == null)
            return;

        SelectedBuildProject.DisplayName = DisplayName;

        await _packManager.Projects.UpdateBuildProjectAsync(SelectedBuildProject);
    }

    private void LoadData()
    {
        IsProcessing = true;
        ExecuteFromNewThread(async () =>
        {
            try
            {
                _buildProjects = await _packManager.Projects.GetBuildProjectsAsync();

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    BuildProjects =
                        new ObservableCollectionExtended<IBuildProject>(_buildProjects.OrderBy(c => c.Name));
                    BuildProjectsIsEmpty = BuildProjects.Count == 0;
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
                await Dispatcher.UIThread.InvokeAsync(() => { IsProcessing = false; });
            }
        });
    }
}