using CommunityToolkit.Mvvm.ComponentModel;

namespace SampleApp.Models;

public sealed partial class Crumb(string title, string? icon = null) : ObservableObject
{
    [ObservableProperty]
    public partial string Title { get; set; } = title;

    public string? Icon { get; } = icon;
}