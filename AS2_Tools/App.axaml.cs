using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AS2_Tools.ViewModels;
using AS2_Tools.Views;

namespace AS2_Tools
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new Launcher()
                {
                    DataContext = new LauncherViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
