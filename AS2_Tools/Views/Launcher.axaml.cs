using System;
using System.Threading.Tasks;
using AS2_Tools.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace AS2_Tools.Views;

public partial class Launcher : Window
{
    public Launcher()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        var dContext = this.DataContext as LauncherViewModel;
        _ = Dispatcher.UIThread.InvokeAsync(dContext!.InitializeAsync);
    }
}