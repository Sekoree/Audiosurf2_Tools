using System.Diagnostics;
using System.Windows.Input;
using AS2_Tools.Controls;
using AS2_Tools.Interfaces;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AS2_Tools.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    [Reactive] public bool IsPaneOpen { get; set; } = true;

    [Reactive] public MoreFoldersViewModel MoreFoldersViewModel { get; set; } = new();
    [Reactive] public AboutViewModel AboutViewModel { get; set; } = new();
    
    [Reactive] public PaneButton? CurrentSelection { get; set; }
    
    [Reactive] ICommand ChangePageCommand { get; set; }
    
    public MainWindowViewModel()
    {
        Debug.WriteLine("Creating MainWindowViewModel");
        ChangePageCommand = ReactiveCommand.Create<PaneButton>(OpenPage);
    }
    
    public void OpenClosePane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
    
    public void OpenPage(PaneButton paneButton)
    {
        if (CurrentSelection?.PageViewModel  != null) 
            CurrentSelection.IsSelected = false;
        
        paneButton.PageContent.DataContext ??= paneButton.PageViewModel;
        paneButton.IsSelected = true;
        CurrentSelection = paneButton;
    }
}
