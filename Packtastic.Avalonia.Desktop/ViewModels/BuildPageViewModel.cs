using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using Avalonia.Threading;
using DynamicData.Binding;
using Microsoft.Build.Exceptions;
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
    [Reactive] public string DisplayName { get; set; }
    [Reactive] public string SlugName { get; set; }

    public BuildPageViewModel(PackManager packManager)
    {
        _packManager = packManager;

        this.WhenAnyValue(vm => vm.DisplayName)
            .Subscribe(name => SlugName = name.ToSlug());

        RxApp.MainThreadScheduler.Schedule(LoadData);
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