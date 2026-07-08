namespace SampleApp.Models;

public sealed class Crumb(string title, string? icon = null)
{
    public string Title { get; } = title;

    public string? Icon { get; } = icon;
}