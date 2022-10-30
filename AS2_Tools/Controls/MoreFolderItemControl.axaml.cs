using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AS2_Tools.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Platform.Storage;
using ReactiveUI;

namespace AS2_Tools.Controls;

public class MoreFolderItemControl : TemplatedControl
{
    public static readonly StyledProperty<MoreFolderItemModel> MoreFolderItemProperty =
        AvaloniaProperty.Register<MoreFolderItemControl, MoreFolderItemModel>(
            nameof(MoreFolderItem));

    public static readonly StyledProperty<object?> BrowseWindowParameterProperty = AvaloniaProperty.Register<MoreFolderItemControl, object?>(
        nameof(BrowseWindowParameter));

    public object? BrowseWindowParameter
    {
        get => GetValue(BrowseWindowParameterProperty);
        set => SetValue(BrowseWindowParameterProperty, value);
    }

    public static readonly DirectProperty<MoreFolderItemControl, ICommand?> UpButtonCommandProperty =
        AvaloniaProperty.RegisterDirect<MoreFolderItemControl, ICommand?>(
            nameof(UpButtonCommand), o => o.UpButtonCommand, (o, v) => o.UpButtonCommand = v);

    public static readonly DirectProperty<MoreFolderItemControl, ICommand?> DownButtonCommandProperty =
        AvaloniaProperty.RegisterDirect<MoreFolderItemControl, ICommand?>(
            nameof(DownButtonCommand), o => o.DownButtonCommand, (o, v) => o.DownButtonCommand = v);

    public static readonly DirectProperty<MoreFolderItemControl, ICommand?> RemoveButtonCommandProperty =
        AvaloniaProperty.RegisterDirect<MoreFolderItemControl, ICommand?>(
            nameof(RemoveButtonCommand), o => o.RemoveButtonCommand, (o, v) => o.RemoveButtonCommand = v);

    public static readonly DirectProperty<MoreFolderItemControl, ICommand?> BrowseButtonCommandProperty =
        AvaloniaProperty.RegisterDirect<MoreFolderItemControl, ICommand?>(
            nameof(BrowseButtonCommand), o => o.BrowseButtonCommand, (o, v) => o.BrowseButtonCommand = v);

    //DirectProperty ICommand? EditCommandProperty that calls MoreFolderItem.ChangeEditingState
    public static readonly DirectProperty<MoreFolderItemControl, ICommand?> EditCommandProperty =
        AvaloniaProperty.RegisterDirect<MoreFolderItemControl, ICommand?>(
            nameof(EditCommand),
            o => o.EditCommand,
            (o, v) => o.EditCommand = v,
            enableDataValidation: true);

    //DirectProperty ICommand? BrowseButtonCommand
    private ICommand? _browseButtonCommand;

    //DirectProperty ICommand? DownButtonCommand
    private ICommand? _downButtonCommand;

    private ICommand? _editCommand;

    //DirectProperty ICommand? RemoveButtonCommand
    private ICommand? _removeButtonCommand;

    //DirectProperty ICommand? UpButtonCommand
    private ICommand? _upButtonCommand;

    public MoreFolderItemControl()
    {
        EditCommand = ReactiveCommand.Create(() => MoreFolderItem.ChangeEditingState());
        UpButtonCommand = ReactiveCommand.Create(MoveUpOrIncrement);
        DownButtonCommand = ReactiveCommand.Create(MoveDownOrDecrement);
        RemoveButtonCommand = ReactiveCommand.Create(() => MoreFolderItem.Remove());
        BrowseButtonCommand = ReactiveCommand.CreateFromTask<Window>(BrowseForNewPathAsync);

        this.WhenAnyValue(x => x.MoreFolderItem.IsEditing)
            .Subscribe(value => Classes.Set("editing", value));
    }
    
    public async Task BrowseForNewPathAsync(Window parentWindow)
    {
        var folderPickerOptions = new FolderPickerOpenOptions()
        {
            AllowMultiple = false,
            Title = "Select a folder"
        };
        var folderResult = await parentWindow.StorageProvider.OpenFolderPickerAsync(folderPickerOptions);
        if (folderResult.Any())
        {
            var gorUri = folderResult[0].TryGetUri(out var uri);
            if (gorUri)
            {
                MoreFolderItem.Path = uri!.LocalPath;
            }
        }
    }
    
    public void MoveUpOrIncrement()
    {
        if (MoreFolderItem.IsEditing)
        {
            ItemPosition++;
            RaisePropertyChanged(ItemPositionProperty, ItemPosition - 1, ItemPosition);
        }
        else
            MoreFolderItem.MoveUp();
    }
    
    public void MoveDownOrDecrement()
    {
        if (!MoreFolderItem.IsEditing)
            MoreFolderItem.MoveDown();
        else if (MoreFolderItem.Position > -1)
        {
            ItemPosition--;
            RaisePropertyChanged(ItemPositionProperty, ItemPosition + 1, ItemPosition);
        }
    }

    public ICommand? UpButtonCommand
    {
        get => _upButtonCommand;
        set => SetAndRaise(UpButtonCommandProperty, ref _upButtonCommand, value);
    }

    public ICommand? DownButtonCommand
    {
        get => _downButtonCommand;
        set => SetAndRaise(DownButtonCommandProperty, ref _downButtonCommand, value);
    }

    public ICommand? RemoveButtonCommand
    {
        get => _removeButtonCommand;
        set => SetAndRaise(RemoveButtonCommandProperty, ref _removeButtonCommand, value);
    }

    public ICommand? BrowseButtonCommand
    {
        get => _browseButtonCommand;
        set => SetAndRaise(BrowseButtonCommandProperty, ref _browseButtonCommand, value);
    }

    public MoreFolderItemModel MoreFolderItem
    {
        get => GetValue(MoreFolderItemProperty);
        set => SetValue(MoreFolderItemProperty, value);
    }

    public ICommand? EditCommand
    {
        get => _editCommand;
        set => SetAndRaise(EditCommandProperty, ref _editCommand, value);
    }


    #region Forwarded Properties

    public static readonly DirectProperty<MoreFolderItemControl, int> ItemPositionProperty =
        AvaloniaProperty.RegisterDirect<MoreFolderItemControl, int>(
            nameof(Name),
            o => o.ItemPosition,
            (o, v) => o.ItemPosition = v);


    public static readonly DirectProperty<MoreFolderItemControl, string> ItemNameProperty =
        AvaloniaProperty.RegisterDirect<MoreFolderItemControl, string>(
            nameof(ItemName),
            o => o.ItemName,
            (o, v) => o.ItemName = v);

    public static readonly DirectProperty<MoreFolderItemControl, string> ItemPathProperty =
        AvaloniaProperty.RegisterDirect<MoreFolderItemControl, string>(
            nameof(ItemPath),
            o => o.ItemPath,
            (o, v) => o.ItemPath = v);

    public int ItemPosition
    {
        get => MoreFolderItem.Position;
        set => MoreFolderItem.Position = value;
    }

    public string ItemName
    {
        get => MoreFolderItem.Name;
        set => MoreFolderItem.Name = value;
    }

    public string ItemPath
    {
        get => MoreFolderItem.Path;
        set => MoreFolderItem.Path = value;
    }

    #endregion
}