using System;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;

namespace Packtastic.Avalonia.Desktop.ViewModels.Base;

public class PageViewModelBase : ViewModelBase
{
    [Reactive] public bool IsProcessing { get; set; }
    
    protected Task ExecuteFromNewThread(Func<Task> func)
    {
        var tcs = new TaskCompletionSource<object?>();

        async void RunThreadTask()
        {
            try
            {
                await func();
                tcs.SetResult(null);
            }
            catch (Exception exception)
            {
                tcs.SetException(exception);
            }
        }
        
        var thread = new Thread(RunThreadTask);
        thread.IsBackground = true;
        thread.Start();

        return tcs.Task;
    }
}