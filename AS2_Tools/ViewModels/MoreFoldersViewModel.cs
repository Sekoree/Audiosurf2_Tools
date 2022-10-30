using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using AS2_Tools.Interfaces;
using AS2_Tools.Models;
using AS2_Tools.Utilities;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AS2_Tools.ViewModels;

public class MoreFoldersViewModel : ViewModelBase, IPageViewModel
{
    private readonly MoreFoldersManager _moreFoldersManager = new();
    
    [Reactive] public bool IsSelected { get; set; }
    
    [Reactive] public ObservableCollection<MoreFolderItemModel>? MoreFoldersItems { get; set; }
    
    [Reactive] public ICommand AddFolderCommand { get; set; }

    public MoreFoldersViewModel()
    {
        AddFolderCommand = ReactiveCommand.CreateFromTask<Window>(AddItemAsync);
    }
    
    public async Task InitializeAsync()
    {
        await ReloadFromFileAsync();
    }
    
    public async Task ReloadFromFileAsync()
    {
        MoreFoldersItems = await _moreFoldersManager.LoadMoreFolderItemsFromFileAsync();
    }

    public async Task AddItemAsync(Window window)
    {
        var folderPickerOptions = new FolderPickerOpenOptions()
        {
            AllowMultiple = false,
            Title = "Select a folder to add"
        };
        var folders = await window.StorageProvider.OpenFolderPickerAsync(folderPickerOptions);
        if (folders.Count == 0)
            return;
        if (!folders[0].TryGetUri(out var folderUri))
            return;
        var localPath = folderUri.LocalPath;
        if (!Directory.Exists(localPath))
            return;
        var newMoreFoldersItem = new MoreFolderItemModel()
        {
            Path = localPath
        };
        MoreFoldersItems?.Add(newMoreFoldersItem);
    }
    
    public void RemoveItem(MoreFolderItemModel item)
    {
        MoreFoldersItems?.Remove(item);
    }
    
    public async Task SaveToFileAsync()
    {
        if (MoreFoldersItems is null)
            return;
        await _moreFoldersManager.SaveMoreFolderItemsToFileAsync(MoreFoldersItems);
    }
}