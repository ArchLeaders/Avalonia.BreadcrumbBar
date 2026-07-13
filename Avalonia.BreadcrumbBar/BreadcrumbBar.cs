using System.Windows.Input;
using Avalonia.BreadcrumbBar.Interactivity;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Metadata;

namespace Avalonia.BreadcrumbBar;

[TemplatePart("PART_OverflowGrid", typeof(Grid), IsRequired = true)]
[TemplatePart("PART_OverflowButton", typeof(Button), IsRequired = true)]
[TemplatePart("PART_OverflowChevronButton", typeof(Button), IsRequired = true)]
[PseudoClasses(":overflow")]
public sealed class BreadcrumbBar : ItemsControl
{
    public static readonly RoutedEvent<BreadcrumbBarItemClickedEventArgs> ItemClickedEvent =
        RoutedEvent.Register<BreadcrumbBar, BreadcrumbBarItemClickedEventArgs>(nameof(ItemClicked), RoutingStrategies.Bubble);
    
    public static readonly StyledProperty<IDataTemplate?> ItemIconTemplateProperty =
        AvaloniaProperty.Register<BreadcrumbBarItem, IDataTemplate?>(nameof(ItemIconTemplate));
    
    public static readonly StyledProperty<BindingBase?> ItemIconBindingProperty =
        AvaloniaProperty.Register<BreadcrumbBarItem, BindingBase?>(nameof(ItemIconBinding));

    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<BreadcrumbBar, ICommand?>(nameof(Command), enableDataValidation: true);

    private Grid? _overflowGrid;
    private readonly MenuFlyout _overflowFlyout = new() {
        Placement = PlacementMode.BottomEdgeAlignedLeft
    };

    /// <summary>
    /// Raised when the user clicks the button.
    /// </summary>
    public event EventHandler<BreadcrumbBarItemClickedEventArgs>? ItemClicked {
        add => AddHandler(ItemClickedEvent, value);
        remove => RemoveHandler(ItemClickedEvent, value);
    }
    
    public IDataTemplate? ItemIconTemplate {
        get => GetValue(ItemIconTemplateProperty);
        set => SetValue(ItemIconTemplateProperty, value);
    }
    
    [AssignBinding]
    [InheritDataTypeFromItems("ItemsSource")]
    public BindingBase ItemIconBinding {
        get => GetValue(ItemIconBindingProperty) ?? new Binding();
        set => SetValue(ItemIconBindingProperty, value);
    }

    public ICommand? Command {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new BreadcrumbBarItem {
            [!BreadcrumbBarItem.IconProperty] = ItemIconBinding,
            IconTemplate = ItemIconTemplate
        };
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<BreadcrumbBarItem>(item, out recycleKey);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _overflowGrid = e.NameScope.Get<Grid>("PART_OverflowGrid");
        e.NameScope.Get<Button>("PART_OverflowButton").Flyout = _overflowFlyout;
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);

        UpdateOverflow(e.NewSize.Width);
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

    internal void UpdateOverflow(double targetMaxWidth)
    {
        bool isOverflow = false;
        var size = _overflowGrid?.Bounds.Width ?? 0;

        foreach (var item in LogicalChildren.OfType<BreadcrumbBarItem>().Reverse()) {
            size += item.Bounds.Width;
            item.IsVisible = !(isOverflow = size > targetMaxWidth);
        }

        UpdateOverflowFlyout();
        PseudoClasses.Set(":overflow", isOverflow);
    }

    private void UpdateOverflowFlyout()
    {
        _overflowFlyout.Items.Clear();

        foreach (var item in LogicalChildren.OfType<BreadcrumbBarItem>().Where(item => !item.IsVisible).Reverse()) {
            var menuItem = new MenuItem {
                Header = item.Content,
                HeaderTemplate = item.ContentTemplate,
                Icon = new ContentPresenter {
                    Content = item.Icon,
                    ContentTemplate = item.IconTemplate
                },
                Command = item.Command,
                CommandParameter = item.CommandParameter
            };
            
            menuItem.Click += (s, _) => RaiseItemClicked(s, item);

            _overflowFlyout.Items.Add(menuItem);
        }
    }
}