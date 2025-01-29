namespace Packtastic.Avalonia;

public interface IProject
{
    public string Name { get; }
    string AbsolutePath { get; }
    bool AnyPackage(string packageName);
}