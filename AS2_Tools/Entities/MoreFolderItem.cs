using AS2_Tools.Models;
using Mapster;

namespace AS2_Tools.Entities;

public class MoreFolderItem
{
    public int Position { get; set; } = -1;
    public string Name { get; set; } = "Untitled";
    public string Path { get; set; } = string.Empty;

    public MoreFolderItemModel ToModel() => this.Adapt<MoreFolderItemModel>();
}