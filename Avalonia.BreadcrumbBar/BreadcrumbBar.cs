using Avalonia.Controls;

namespace Avalonia.BreadcrumbBar;

public sealed class BreadcrumbBar : ItemsControl
{
    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new BreadcrumbBarItem();
    }
    
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<BreadcrumbBarItem>(item, out recycleKey);
    }
}