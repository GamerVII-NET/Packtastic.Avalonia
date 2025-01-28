namespace Packtastic.Avalonia.Models;

public class SolutionSln(string filePath) : ISolution
{
    public string Name { get; } = Path.GetFileNameWithoutExtension(filePath);

    public override string ToString()
    {
        return $"Solution: {Name}";
    }
}