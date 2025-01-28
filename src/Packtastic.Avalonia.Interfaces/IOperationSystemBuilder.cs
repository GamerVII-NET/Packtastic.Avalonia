namespace Packtastic.Avalonia;

public interface IOperationSystemBuilder
{
    bool BuildProject(string projectPath, string configuration = "Release");
}