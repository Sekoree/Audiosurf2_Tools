﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AS2_Tools.Controls;

public partial class MoreFoldersView : UserControl
{
    public MoreFoldersView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}