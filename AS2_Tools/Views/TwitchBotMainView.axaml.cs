using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AS2_Tools.Views;

public partial class TwitchBotMainView : UserControl
{
    public TwitchBotMainView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}