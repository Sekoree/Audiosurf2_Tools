using System.Threading.Tasks;
using AS2_Tools.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace AS2_Tools.ViewModels;

public class MoreFoldersViewModel : ViewModelBase, IPageViewModel
{
    [Reactive] public bool IsSelected { get; set; }
    
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}