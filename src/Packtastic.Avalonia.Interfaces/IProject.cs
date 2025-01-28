namespace Packtastic.Avalonia;

public interface IProject
{
    string Name { get; }
    string AbsolutePath { get; }
    bool AnyPackage(string packageName);
}