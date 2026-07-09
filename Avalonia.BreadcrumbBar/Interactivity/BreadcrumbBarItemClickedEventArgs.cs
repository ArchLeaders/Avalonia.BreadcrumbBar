using Avalonia.Interactivity;

namespace Avalonia.BreadcrumbBar.Interactivity;

public sealed class BreadcrumbBarItemClickedEventArgs(RoutedEvent? routedEvent, BreadcrumbBarItem item) : RoutedEventArgs(routedEvent)
{
    public BreadcrumbBarItem Item { get; } = item;
}