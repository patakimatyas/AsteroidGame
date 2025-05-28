using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AsteroidGame.Model;
using AsteroidGame.Persistence;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using AsteroidGameAvalonia.Views;
using AsteroidGameAvalonia.ViewModels;
using Avalonia.Controls;
using System.Threading.Tasks;
using Avalonia.Data.Core.Plugins;
using Avalonia.Platform;

namespace AsteroidGameAvalonia;

public partial class App : Application
{
    #region Fields

    private Game _model = null!;
    private AsteroidGameViewModel _viewModel = null!;
    private DispatcherTimer _timer = null!;

    #endregion

    #region Properties

    private TopLevel? TopLevel
    {
        get
        {
            return ApplicationLifetime switch
            {
                IClassicDesktopStyleApplicationLifetime desktop => TopLevel.GetTopLevel(desktop.MainWindow),
                ISingleViewApplicationLifetime singleViewPlatform => TopLevel.GetTopLevel(singleViewPlatform.MainView),
                _ => null
            };
        }
    }

    #endregion

    #region Application methods

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {

            desktop.MainWindow = new MainWindow
            {

                DataContext = _viewModel

            };
            InitializeGame((int)desktop.MainWindow.Width - 60, (int)desktop.MainWindow.Height - 60);

        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = _viewModel
            };

            InitializeGame((int)singleViewPlatform.MainView.Width - 60, (int)singleViewPlatform.MainView.Height - 60);

            if (Application.Current?.TryGetFeature<IActivatableLifetime>() is { } activatableLifetime)
            {
                activatableLifetime.Activated += async (sender, args) =>
                {
                    try
                    {
                        await LoadSuspendedGameAsync();
                    }
                    catch
                    {
                    }
                };

                activatableLifetime.Deactivated += async (sender, args) =>
                {
                    try
                    {
                        await SaveSuspendedGameAsync();
                    }
                    catch
                    {
                    }
                };
            }
        }

        base.OnFrameworkInitializationCompleted();
        
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16)
        };
        _timer.Tick += Timer_Tick;
    }

    #endregion

    public void InitializeGame(int? width = null, int? height = null, Game? game = null)
    {
        if(game != null)
        {
            _model = game;
        }
        else if (width != null && height != null)
        {
            _model = new Game((int)width, (int)height);
        }
        else
        {
            _model = new Game(740, 540);
        }
        _model.GameOverEvent += Model_GameOver;

        _viewModel = new AsteroidGameViewModel(_model);
        _viewModel.NewGame += ViewModel_NewGame;
        _viewModel.LoadGame += ViewModel_LoadGame;
        _viewModel.SaveGame += ViewModel_SaveGame;
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow != null)
            {
            desktop.MainWindow.DataContext = _viewModel;
            }
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            if(singleViewPlatform.MainView != null)
            {

                singleViewPlatform.MainView.DataContext = _viewModel;
            }
        }
        
    }

    #region ViewModel event handlers

    private void ViewModel_NewGame(object? sender, EventArgs e)
    {
        InitializeGame(_model.ScreenWidth, _model.ScreenHeight);
        _timer.Start();
        _model.StartGame();
    }

    private async void ViewModel_LoadGame(object? sender, EventArgs e)
    {
        if (TopLevel == null)
        {
            await ShowMessageBox("Asteroid Game", "File handling is not supported!", ButtonEnum.Ok, Icon.Error);
            return;
        }

        try
        {
            var files = await TopLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Load Game",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Asteroid Game Save")
                    {
                        Patterns = new[] { "*.json" }
                    }
                }
            });

            if (files.Count > 0)
            {
                IGamePersistence persistence = new FileGamePersistence();
                var filePath = files[0].Path.LocalPath;
                Game loadedGame = await persistence.LoadGameAsync(filePath);
                InitializeGame(null,null,loadedGame);
                _model.AddTime(loadedGame.ElapsedTime);
                _model.OnGameStateChanged();
                _timer.Start();
            }
        }
        catch (Exception ex)
        {
            await ShowMessageBox("Asteroid Game", "Failed to load the game: " + ex.Message, ButtonEnum.Ok, Icon.Error);
        }
    }

    private async void ViewModel_SaveGame(object? sender, EventArgs e)
    {

        if (TopLevel == null)
        {
            await ShowMessageBox("Asteroid Game", "File handling is not supported!", ButtonEnum.Ok, Icon.Error);
            return;
        }

        try
        {
            var file = await TopLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save Game",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("Asteroid Game Save")
                    {
                        Patterns = new[] { "*.json" }
                    }
                }
            });

            if (file != null)
            {
                IGamePersistence persistence = new FileGamePersistence();
                var filePath = file.Path.LocalPath;
                await persistence.SaveGameAsync(_model, filePath);
            }
        }
        catch (Exception ex)
        {
            await ShowMessageBox("Asteroid Game", "Failed to save the game: " + ex.Message, ButtonEnum.Ok, Icon.Error);
        }
    }

    #endregion

    #region Model event handlers

    private async void Model_GameOver(object? sender, EventArgs e)
    {
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            await ShowMessageBox("Asteroid Game", "Game Over! Elapsed time: " + _model.ElapsedTime.ToString(@"mm\:ss"), ButtonEnum.Ok, Icon.Info);
        });
    }

    #endregion

    #region Helper methods

    private async Task ShowMessageBox(string title, string message, ButtonEnum buttons, Icon icon)
    {
        await MessageBoxManager.GetMessageBoxStandard(title, message, buttons, icon).ShowAsync();
    }

    private async Task LoadSuspendedGameAsync()
    {
        IGamePersistence persistence = new FileGamePersistence();
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AsteroidSuspendedGame.json");

        if (File.Exists(path))
        {
            try
            {
                _model = await persistence.LoadGameAsync(path);
                _viewModel = new AsteroidGameViewModel(_model);
                _model.OnGameStateChanged();
            }
            catch
            {
            }
        }
    }

    private async Task SaveSuspendedGameAsync()
    {
        IGamePersistence persistence = new FileGamePersistence();
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AsteroidSuspendedGame.json");

        try
        {
            await persistence.SaveGameAsync(_model, path);
        }
        catch
        {
        }
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        
        _model.Update();
    }



    #endregion
}
