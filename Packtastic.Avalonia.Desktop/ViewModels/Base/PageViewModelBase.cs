using System;
using System.Threading;
using System.Threading.Tasks;

namespace Packtastic.Avalonia.Desktop.ViewModels.Base;

public class PageViewModelBase : ViewModelBase
{
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

        new Thread(RunThreadTask).Start();

        return tcs.Task;
    }
}