using System.Windows.Input;
using Avalonia.BreadcrumbBar.Interactivity;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Path = Avalonia.Controls.Shapes.Path;

namespace Avalonia.BreadcrumbBar;

[TemplatePart("PART_Button", typeof(Button), IsRequired = true)]
[TemplatePart("PART_Icon", typeof(ContentPresenter))]
[TemplatePart("PART_ContentPresenter", typeof(ContentPresenter))]
[TemplatePart("PART_FlyoutButton", typeof(Button))]
[TemplatePart("PART_ChevronIcon", typeof(Viewbox))]
[TemplatePart("PART_ChevronPath", typeof(Path))]
[PseudoClasses(":hasflyout")]
public sealed class BreadcrumbBarItem : ContentControl
{
    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<BreadcrumbBarItem, object?>(nameof(Icon));

    public static readonly StyledProperty<Thickness> IconPaddingProperty =
        AvaloniaProperty.Register<BreadcrumbBarItem, Thickness>(nameof(IconPadding));

    public static readonly StyledProperty<IDataTemplate?> IconTemplateProperty =
        AvaloniaProperty.Register<BreadcrumbBarItem, IDataTemplate?>(nameof(IconTemplate));

    public static readonly StyledProperty<FlyoutBase?> FlyoutProperty =
        AvaloniaProperty.Register<BreadcrumbBarItem, FlyoutBase?>(nameof(Flyout));

    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<BreadcrumbBarItem, ICommand?>(nameof(Command), enableDataValidation: true);

    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<BreadcrumbBarItem, object?>(nameof(CommandParameter));

    public object? Icon {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public Thickness IconPadding {
        get => GetValue(IconPaddingProperty);
        set => SetValue(IconPaddingProperty, value);
    }

    public IDataTemplate? IconTemplate {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public FlyoutBase? Flyout {
        get => GetValue(FlyoutProperty);
        set => SetValue(FlyoutProperty, value);
    }

    public ICommand? Command {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == FlyoutProperty) {
            PseudoClasses.Set(":hasflyout", change.NewValue is not null);
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        e.NameScope.Get<Button>("PART_Button").Click += OnItemClicked;
    }
    
    private void OnItemClicked(object? sender, RoutedEventArgs e)
    {
        if (this.GetLogicalParent<BreadcrumbBar>() is not { } parent) {
            return;
        }

        parent.RaiseItemClicked(sender, this);
    }
}