using System.Collections.ObjectModel;
using AS2_Tools.Entities;
using Mapster;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AS2_Tools.Models;

public class MoreFolderItemModel : ReactiveObject
{
    [Reactive] public int Position { get; set; } = -1;
    [Reactive] public string Name { get; set; } = "Untitled";
    [Reactive] public string Path { get; set; } = string.Empty;
    [Reactive] public bool IsEditing { get; set; } = false;
    
    [Reactive] public ObservableCollection<MoreFolderItemModel>? ParentCollection { get; set; }

    public void ChangeEditingState()
    {
        IsEditing = !IsEditing;
    }
    
    public MoreFolderItem ToEntity() => this.Adapt<MoreFolderItem>();

    public void MoveUp()
    {
        var index = ParentCollection!.IndexOf(this);
        if (index == 0) return;
        ParentCollection.Move(index, index - 1);
    }
    
    public void MoveDown()
    {
        var index = ParentCollection!.IndexOf(this);
        if (index == ParentCollection.Count - 1) return;
        ParentCollection.Move(index, index + 1);
    }
    
    public void Remove()
    {
        ParentCollection!.Remove(this);
    }
}