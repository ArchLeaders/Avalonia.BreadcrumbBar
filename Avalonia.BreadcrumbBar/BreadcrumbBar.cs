using System.Windows.Input;
using Avalonia.BreadcrumbBar.Interactivity;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Avalonia.BreadcrumbBar;

public sealed class BreadcrumbBar : ItemsControl
{
    public static readonly RoutedEvent<BreadcrumbBarItemClickedEventArgs> ItemClickedEvent =
        RoutedEvent.Register<BreadcrumbBar, BreadcrumbBarItemClickedEventArgs>(nameof(ItemClicked), RoutingStrategies.Bubble);
    
    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<BreadcrumbBar, ICommand?>(nameof(Command), enableDataValidation: true);

    /// <summary>
    /// Raised when the user clicks the button.
    /// </summary>
    public event EventHandler<BreadcrumbBarItemClickedEventArgs>? ItemClicked {
        add => AddHandler(ItemClickedEvent, value);
        remove => RemoveHandler(ItemClickedEvent, value);
    }
    
    public ICommand? Command {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new BreadcrumbBarItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<BreadcrumbBarItem>(item, out recycleKey);
    }

    public void RaiseItemClicked(object? sender, BreadcrumbBarItem item)
    {
        var e = new BreadcrumbBarItemClickedEventArgs(ItemClickedEvent, item) {
            Source = sender
        };

        RaiseEvent(e);

        var command = Command;
        if (!e.Handled && command?.CanExecute(item) is true) {
            command.Execute(item);
            e.Handled = true;
        }
    }
}