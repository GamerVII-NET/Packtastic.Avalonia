using System.Diagnostics;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Logging;

namespace Packtastic.Avalonia.Core.Factories;

public class WindowsSystemBuilder(string platform) : SystemBuilderBase("win-", platform)
{
    
}