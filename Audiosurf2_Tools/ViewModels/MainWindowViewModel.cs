﻿using System;
using System.Collections.Generic;
using System.Text;
using Audiosurf2_Tools.Models;
using Avalonia.Media;
using ReactiveUI.Fody.Helpers;

namespace Audiosurf2_Tools.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    [Reactive] public InstallerViewModel InstallerViewModel { get; set; }
    [Reactive] public MoreFoldersViewModel MoreFoldersViewModel { get; set; }
    [Reactive] public PlaylistEditorViewModel PlaylistEditorViewModel { get; set; }
    [Reactive] public TwitchBotViewModel TwitchBotViewModel { get; set; }
    [Reactive] public SettingsViewModel SettingsViewModel { get; set; }
    [Reactive] public bool OpenSidebar { get; set; } = true;
    [Reactive] public ISolidColorBrush InstallerHighlight { get; set; }
    [Reactive] public ISolidColorBrush MoreFoldersHighlight { get; set; }
    [Reactive] public ISolidColorBrush PlaylistEditorHighlight { get; set; }
    [Reactive] public ISolidColorBrush TwitchBotHighlight { get; set; }
    [Reactive] public ISolidColorBrush SettingsHighlight { get; set; }

    public MainWindowViewModel()
    {
        InstallerViewModel = new();
        MoreFoldersViewModel = new();
        PlaylistEditorViewModel = new();
        TwitchBotViewModel = new();
        SettingsViewModel = new();
        InstallerHighlight = Brushes.Transparent;
        MoreFoldersHighlight = Brushes.Transparent;
        PlaylistEditorHighlight = Brushes.Transparent;
        TwitchBotHighlight = Brushes.Transparent;
        SettingsHighlight = Brushes.Transparent;
        OpenTwitchBot();
    }

    public void OpenCloseSidebar()
        => OpenSidebar = !OpenSidebar;

    public void OpenInstaller()
    {
        (InstallerHighlight, MoreFoldersHighlight, PlaylistEditorHighlight, TwitchBotHighlight, SettingsHighlight) =
            (SolidColorBrush.Parse("#33ffffff"), Brushes.Transparent, Brushes.Transparent, Brushes.Transparent, Brushes.Transparent);
        (InstallerViewModel.IsOpen, MoreFoldersViewModel.IsOpen, PlaylistEditorViewModel.IsOpen,
                TwitchBotViewModel.IsOpen, SettingsViewModel.IsOpen) =
            (true, false, false, false, false);
    }

    public void OpenMoreFolders()
    {
        (InstallerHighlight, MoreFoldersHighlight, PlaylistEditorHighlight, TwitchBotHighlight, SettingsHighlight) =
            (Brushes.Transparent, SolidColorBrush.Parse("#33ffffff"), Brushes.Transparent, Brushes.Transparent, Brushes.Transparent);
        (InstallerViewModel.IsOpen, MoreFoldersViewModel.IsOpen, PlaylistEditorViewModel.IsOpen,
                TwitchBotViewModel.IsOpen, SettingsViewModel.IsOpen) =
            (false, true, false, false, false);
    }

    public void OpenPlaylistEditor()
    {
        (InstallerHighlight, MoreFoldersHighlight, PlaylistEditorHighlight, TwitchBotHighlight, SettingsHighlight) =
            (Brushes.Transparent, Brushes.Transparent, SolidColorBrush.Parse("#33ffffff"), Brushes.Transparent, Brushes.Transparent);
        (InstallerViewModel.IsOpen, MoreFoldersViewModel.IsOpen, PlaylistEditorViewModel.IsOpen,
                TwitchBotViewModel.IsOpen, SettingsViewModel.IsOpen) =
            (false, false, true, false, false);
    }

    public void OpenTwitchBot()
    {
        (InstallerHighlight, MoreFoldersHighlight, PlaylistEditorHighlight, TwitchBotHighlight, SettingsHighlight) =
            (Brushes.Transparent, Brushes.Transparent, Brushes.Transparent, SolidColorBrush.Parse("#33ffffff"), Brushes.Transparent);
        (InstallerViewModel.IsOpen, MoreFoldersViewModel.IsOpen, PlaylistEditorViewModel.IsOpen,
                TwitchBotViewModel.IsOpen, SettingsViewModel.IsOpen) =
            (false, false, false, true, false);
    }
    
    public void OpenSettings()
    {
        (InstallerHighlight, MoreFoldersHighlight, PlaylistEditorHighlight, TwitchBotHighlight, SettingsHighlight) =
            (Brushes.Transparent, Brushes.Transparent, Brushes.Transparent, Brushes.Transparent, SolidColorBrush.Parse("#33ffffff"));
        (InstallerViewModel.IsOpen, MoreFoldersViewModel.IsOpen, PlaylistEditorViewModel.IsOpen,
                TwitchBotViewModel.IsOpen, SettingsViewModel.IsOpen) =
            (false, false, false, false, true);
    }
}