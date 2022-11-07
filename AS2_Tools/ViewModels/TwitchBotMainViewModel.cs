using System.Threading.Tasks;
using AS2_Tools.Interfaces;

namespace AS2_Tools.ViewModels;

public class TwitchBotMainViewModel : ViewModelBase, IPageViewModel
{
    public bool IsSelected { get; set; }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}