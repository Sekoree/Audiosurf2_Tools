using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AS2_Tools.Entities;
using AS2_Tools.Models;
using Mapster;

namespace AS2_Tools.Utilities;

public class MoreFoldersManager
{
    public async Task<ObservableCollection<MoreFolderItemModel>?> LoadMoreFolderItemsFromFileAsync()
    {
        var path = Path.Combine(GameUtils.GamePath, "MoreFolders.json");
        if (!File.Exists(path))
            return new ObservableCollection<MoreFolderItemModel>();
        
        var json = await File.ReadAllTextAsync(path);
        var items = JsonSerializer.Deserialize<ObservableCollection<MoreFolderItemModel>>(json);
        if (items == default)
            return new ObservableCollection<MoreFolderItemModel>();

        foreach (var item in items)
            item.ParentCollection = items;
        
        return items;
    }
    
    public async Task SaveMoreFolderItemsToFileAsync(IEnumerable<MoreFolderItemModel> items)
    {
        var path = Path.Combine(GameUtils.GamePath, "MoreFolders.json");
        var modelsAsEntities = items.Adapt<List<MoreFolderItem>>();
        var json = JsonSerializer.Serialize(modelsAsEntities);
        await File.WriteAllTextAsync(path, json);
    }
}