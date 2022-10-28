using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace AS2_Tools;

public class GameUtils
{
    /// <summary>
    /// When in doubt use <see cref="GetGameLocationAsync"/>
    /// </summary>
    public static string GamePath = string.Empty;
    
    public static async Task<string?> GetGameLocationAsync()
    {
        if (!string.IsNullOrEmpty(GamePath))
            return GamePath;

        if (!OperatingSystem.IsWindows())
            return string.Empty;
        
        if (Directory.Exists("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Audiosurf 2"))
            return GamePath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Audiosurf 2";

        try
        {
            //GameID: 235800
            var steamPath = Registry.GetValue(
                                "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Valve\\Steam", "InstallPath", null)
                            ?? throw new KeyNotFoundException("Steam installation not found");
            
            var vdfText = await File.ReadAllLinesAsync(
                Path.Combine(steamPath.ToString() 
                             ?? throw new InvalidOperationException("Invalid Steam install path"), "config\\libraryfolders.vdf"));
            
            var gameLineText = vdfText.FirstOrDefault(x => x.Contains("\"235800\"")) ??
                               throw new DirectoryNotFoundException("Game isn't listed as installed in Steam");
            
            var lineIndex = Array.IndexOf(vdfText, gameLineText);
            for (var i = lineIndex - 1; i >= 0; i--)
            {
                if (!vdfText[i].Contains("\"path\"")) 
                    continue;
                
                vdfText[i] = vdfText[i].Replace("\"path\"", "");
                var libraryPath = vdfText[i][(vdfText[i].IndexOf('\"') + 1)..vdfText[i].LastIndexOf('\"')];
                libraryPath = libraryPath.Replace("\\\\", "\\");
                var directory = Path.Combine(libraryPath, "steamapps\\common\\Audiosurf 2");
                if (!Directory.Exists(directory))
                    return string.Empty;
                
                var dirInfo = new DirectoryInfo(directory);
                var files = dirInfo.GetFiles();

                if (!files.Any(x => x.Name.Contains("Audiosurf2.exe"))) 
                    continue;
                
                return GamePath = directory;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        
        return string.Empty;
    }

    public static async Task<string> GetPatchVersionAsync(string gamePath)
    {
        var versionFile = Path.Combine(gamePath, "Audiosurf2_Data", "Updater",  "version.txt");
        if (!File.Exists(versionFile))
            return string.Empty;

        return await File.ReadAllTextAsync(versionFile);
        
    }
}