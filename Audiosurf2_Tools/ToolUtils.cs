using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Audiosurf2_Tools.Entities;
using Audiosurf2_Tools.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

#if !LINUX
using Microsoft.Win32;
#endif

namespace Audiosurf2_Tools;

public class ToolUtils
{
    public static async Task<string> GetGameDirectoryAsync()
    {
        string? directory = "";
#if LINUX
        var appSettings = Globals.TryGetGlobal<AppSettings>("Settings");
        if (!string.IsNullOrWhiteSpace(appSettings.AS2Location))
        {
            return appSettings.AS2Location;
        }

        var folderSelect = new OpenFolderDialog()
        {
            Title = "Select the Audiosurf 2 directory (Which has Audiosurf2.x86_64)"
        };
        var mainWindow = ((IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!).MainWindow;
        directory = await folderSelect.ShowAsync(mainWindow);
        if (string.IsNullOrWhiteSpace(directory))
        {
            Environment.Exit(1);
        }
        if (!Directory.Exists(directory))
        {
            Environment.Exit(1);
        }
        var hasFile = Directory.GetFiles(directory, "Audiosurf2.x86_64", SearchOption.TopDirectoryOnly).Any();
        if (!hasFile)
        {
            Environment.Exit(1);
        }
        appSettings.AS2Location = directory;
        return directory;
#else   
        if (Directory.Exists("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Audiosurf 2"))
        {
            directory = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Audiosurf 2";
        }
        else
        {
            try
            {
                //GameID: 235800
                object steamPath =
                    Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Valve\\Steam", "InstallPath", null) ??
                    throw new KeyNotFoundException("Steam installation not found");
                var vdfText = await File.ReadAllLinesAsync(Path.Combine(
                    steamPath.ToString() ?? throw new InvalidOperationException("Invalid Steam install path"),
                    "config\\libraryfolders.vdf"));
                var gameLineText = vdfText.FirstOrDefault(x => x.Contains("\"235800\"")) ??
                                   throw new DirectoryNotFoundException("Game isn't listed as installed in Steam");
                var lineIndex = Array.IndexOf(vdfText, gameLineText);
                for (int i = lineIndex - 1; i >= 0; i--)
                {
                    if (vdfText[i].Contains("\"path\""))
                    {
                        vdfText[i] = vdfText[i].Replace("\"path\"", "");
                        var libraryPath = vdfText[i][(vdfText[i].IndexOf('\"') + 1)..vdfText[i].LastIndexOf('\"')];
                        libraryPath = libraryPath.Replace("\\\\", "\\");
                        directory = Path.Combine(libraryPath, "steamapps\\common\\Audiosurf 2");
                        break;
                    }
                }

                if (string.IsNullOrWhiteSpace(directory))
                    throw new DirectoryNotFoundException("Unable to find Audiosurf 2 directory");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        if (string.IsNullOrWhiteSpace(directory)) 
            return directory;
        {
            if (!Directory.Exists(directory))
                return "";
            var dirInfo = new DirectoryInfo(directory);
            var files = dirInfo.GetFiles();
            if (!files.Any(x => x.Name.Contains("Audiosurf2.exe")))
            {
                return "";
            }
        }
#endif
        
        return directory;
    }
}