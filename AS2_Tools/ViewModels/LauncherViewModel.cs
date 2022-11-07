using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using AS2_Tools.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AS2_Tools.ViewModels;

public class LauncherViewModel : ViewModelBase
{
    [Reactive] public string LoadingStatus { get; set; } = string.Empty;


    [Reactive] public bool InstallerNeeded { get; set; } = false;
    [Reactive] public bool InstallerRunning { get; set; } = false;
    [Reactive] public string InstallerStatus { get; set; } = "Ready to install!";
    [Reactive] public double InstallerProgressValue { get; set; } = 0;

    [Reactive] public ICommand InstallCommand { get; init; }

    public LauncherViewModel()
    {
        InstallCommand = ReactiveCommand.CreateFromTask(InstallAsync);
    }

    public async Task InitializeAsync()
    {
        var gameLocation = await GameUtils.GetGameLocationAsync();
        var lifetime = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
        var window = lifetime.MainWindow;
        if (string.IsNullOrEmpty(gameLocation))
        {
            LoadingStatus = "Game not found...";

            //Maybe make suggested starting location Steam folder
            var folderPickerOptions = new FolderPickerOpenOptions()
            {
                AllowMultiple = false,
                Title = "Please select the Audiosurf 2 directory (Where Audiosurf2.exe is located)"
            };
            var result = await window!.StorageProvider.OpenFolderPickerAsync(folderPickerOptions);
            var gameFound = false;
            if (result.Count > 0 && result[0].TryGetUri(out var uri))
            {
                //Get local path from URI
                var localPath = uri.LocalPath;
                if (File.Exists(Path.Combine(localPath, "Audiosurf2.exe")))
                {
                    //Save game location
                    GameUtils.SetGamePath(localPath);
                    LoadingStatus = "Game found!";
                    gameFound = true;
                }
                else
                    gameFound = false;
            }
            if (!gameFound)
            {
                LoadingStatus = "Game not found, exiting";
                await Task.Delay(5000);
                Environment.Exit(1);
                return;
            }
        }

        //Try getting Patch version from Updater location
        var patchVersion = await GameUtils.GetPatchVersionAsync(gameLocation);
        if (string.IsNullOrEmpty(patchVersion))
        {
            InstallerNeeded = true;
            LoadingStatus = "Patch not found";
            return;
        }

        LoadingStatus = "Patch version: " + patchVersion;
        await Task.Delay(2000);
        try
        {
            var vm = new MainWindowViewModel();
            //Init all PageViewModels
            await vm.MoreFoldersViewModel.InitializeAsync();
            await vm.PlaylistEditorViewModel.InitializeAsync();
            await vm.TwitchBotMainViewModel.InitializeAsync();
            await vm.AboutViewModel.InitializeAsync();
            var mainWindow = new MainWindow()
            {
                DataContext = vm
            };
            mainWindow.Show();
            window?.Close();
            lifetime.MainWindow = mainWindow;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task InstallAsync()
    {
        InstallerRunning = true;
        InstallerStatus = "Downloading Patch...";
        var httpClient = new HttpClient();
        var result = await httpClient.GetAsync("https://files.audiosurf2.info/newpatch/audiosurf2_community_patch.zip",
            HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(true);

        if (!result.IsSuccessStatusCode)
        {
            InstallerStatus = "Error downloading patch, exiting...";
            await Task.Delay(5000);
            Environment.Exit(1);
            return;
        }

        var totalBytes = result.Content.Headers.ContentLength;
        await using var stream = await result.Content.ReadAsStreamAsync().ConfigureAwait(true);
        var destinationStream = new MemoryStream();
        //Make buffer and read data
        var buffer = new byte[1024];
        var bytesRead = 0;
        var totalBytesRead = 0;
        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(true)) > 0)
        {
            await destinationStream.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(true);
            totalBytesRead += bytesRead;
            InstallerProgressValue = (totalBytesRead / (double)totalBytes!) * 100;
        }

        destinationStream.Seek(0, SeekOrigin.Begin);
        InstallerStatus = "Extracting Patch...";
        //Extract zip to game location
        var path = await GameUtils.GetGameLocationAsync();
        if (string.IsNullOrEmpty(path))
        {
            InstallerStatus = "Error extracting patch, exiting...";
            await Task.Delay(5000);
            Environment.Exit(1);
            return;
        }
        var zip = new ZipArchive(destinationStream);
        zip.ExtractToDirectory(path, true);
        InstallerStatus = "Patch installed!";
        InstallerNeeded = false;
        InstallerRunning = false;
        await InitializeAsync();
    }
}