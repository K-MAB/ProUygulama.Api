using System.Text.RegularExpressions;

namespace ProUygulama.Api.Helpers;

public static class SlugHelper
{
    public static string Generate(string text)
    {
        text = text.ToLowerInvariant();
        text = Regex.Replace(text, @"[^a-z0-9\s-]", "");
        text = Regex.Replace(text, @"\s+", "-").Trim('-');
        return text;
    }
}