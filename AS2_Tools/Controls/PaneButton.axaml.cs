using System.Collections;
using System.Windows.Input;
using AS2_Tools.Interfaces;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Media;

namespace AS2_Tools.Controls;

[PseudoClasses(":selected")]
public class PaneButton : TemplatedControl
{
    public static readonly DirectProperty<PaneButton, bool> IsSelectedProperty = AvaloniaProperty.RegisterDirect<PaneButton, bool>(
        "IsSelected", o => o.IsSelected, (o, v) => o.IsSelected = v);

    //No idea if this is correct
    public bool IsSelected
    {
        get
        {
            return PageViewModel?.IsSelected == true;
        }
        set
        {
            if (PageViewModel != null) 
                PageViewModel.IsSelected = value;
            
            RaisePropertyChanged(IsSelectedProperty, value, IsSelected);
        }
    }

    public static readonly StyledProperty<UserControl> PageContentProperty = AvaloniaProperty.Register<PaneButton, UserControl>(
        nameof(PageContent), defaultBindingMode: BindingMode.TwoWay);

    public UserControl PageContent
    {
        get => GetValue(PageContentProperty);
        set => SetValue(PageContentProperty, value);
    }
    
    public static readonly StyledProperty<IPageViewModel?> PageViewModelProperty = AvaloniaProperty.Register<PaneButton, IPageViewModel?>(
        nameof(PageViewModel), defaultBindingMode: BindingMode.TwoWay);

    public IPageViewModel? PageViewModel
    {
        get => GetValue(PageViewModelProperty);
        set => SetValue(PageViewModelProperty, value);
    }
    
    public static readonly StyledProperty<Thickness> ImageMarginProperty = AvaloniaProperty.Register<PaneButton, Thickness>(
        nameof(ImageMargin));

    public Thickness ImageMargin
    {
        get => GetValue(ImageMarginProperty);
        set => SetValue(ImageMarginProperty, value);
    }
    
    public static readonly StyledProperty<int> ImageHeightProperty = AvaloniaProperty.Register<PaneButton, int>(
        nameof(ImageHeight), 50);

    public int ImageHeight
    {
        get => GetValue(ImageHeightProperty);
        set => SetValue(ImageHeightProperty, value);
    }

    public static readonly StyledProperty<string> MainTextProperty = AvaloniaProperty.Register<PaneButton, string>(
        nameof(MainText));

    public string MainText
    {
        get => GetValue(MainTextProperty);
        set => SetValue(MainTextProperty, value);
    }

    //StyledProperty IDataTemplate? ItemTemplate
    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
        AvaloniaProperty.Register<PaneButton, IDataTemplate?>(
            nameof(ItemTemplate));

    public IDataTemplate? ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    //StyledProperty IImage ImageSource
    public static readonly StyledProperty<IImage> ImageSourceProperty = AvaloniaProperty.Register<PaneButton, IImage>(
        nameof(ImageSource));

    public IImage ImageSource
    {
        get => GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    private IEnumerable? _items;

    public static readonly DirectProperty<PaneButton, IEnumerable?> ItemsProperty =
        AvaloniaProperty.RegisterDirect<PaneButton, IEnumerable?>(
            nameof(Items), o => o.Items, (o, v) => o.Items = v);

    public IEnumerable? Items
    {
        get => _items;
        set => SetAndRaise(ItemsProperty, ref _items, value);
    }

    //DirectProperty ICommand? ButtonCommand
    public static readonly DirectProperty<PaneButton, ICommand?> ButtonCommandProperty =
        AvaloniaProperty.RegisterDirect<PaneButton, ICommand?>(
            nameof(ButtonCommand), o => o.ButtonCommand, (o, v) => o.ButtonCommand = v);

    private ICommand? _buttonCommand;

    public ICommand? ButtonCommand
    {
        get => _buttonCommand;
        set => SetAndRaise(ButtonCommandProperty, ref _buttonCommand, value);
    }

    public static readonly StyledProperty<object?> ButtonCommandParameterProperty = AvaloniaProperty.Register<PaneButton, object?>(
        nameof(ButtonCommandParameter));

    public object? ButtonCommandParameter
    {
        get => GetValue(ButtonCommandParameterProperty);
        set => SetValue(ButtonCommandParameterProperty, value);
    }

    public static readonly StyledProperty<bool> ShowItemsProperty = AvaloniaProperty.Register<PaneButton, bool>(
        nameof(ShowItems), false);

    public bool ShowItems
    {
        get => GetValue(ShowItemsProperty);
        set => SetValue(ShowItemsProperty, value);
    }

    public PaneButton()
    {
        this.PointerEntered += PaneButton_PointerEntered;
        this.PointerExited += PaneButton_PointerExited;
        IsSelectedProperty.Changed.AddClassHandler<PaneButton>((o, e) => o.IsSelectedChanged(o, e));
    }

    private void IsSelectedChanged(PaneButton paneButton, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is not bool b) 
            return;
        PseudoClasses.Set(":selected", b);
    }

    private void PaneButton_PointerExited(object? sender, PointerEventArgs e)
    {
        ShowItems = false;
    }

    private void PaneButton_PointerEntered(object? sender, PointerEventArgs e)
    {
        ShowItems = true;
    }
}