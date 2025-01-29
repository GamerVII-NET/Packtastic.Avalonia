using System.Text.RegularExpressions;

namespace Packtastic.Avalonia.Core.Helpers;

public static partial class IoHelper
{
    public static string ToSlug(this string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        var slug = name.ToLower();

        slug = Regex.Replace(slug, @"[^a-z0-9]+", "-");

        slug = slug.Trim('-');

        return slug;
    }
}