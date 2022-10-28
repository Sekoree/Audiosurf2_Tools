using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;

namespace AS2_Tools.Interfaces;

public interface IPageViewModel
{
    [Reactive] public bool IsSelected { get; set; }
    
    public Task InitializeAsync();
}