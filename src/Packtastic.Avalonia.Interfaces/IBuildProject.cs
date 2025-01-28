namespace Packtastic.Avalonia;

public interface IBuildProject : IProject
{
    Task BuildAllPlatformsAsync();
}