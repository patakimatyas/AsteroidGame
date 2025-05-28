using AsteroidGame.Model;
using AsteroidGame.Persistence;
using AsteroidGameWPF.View;
using AsteroidGameWPF.ViewModel;
using System.Configuration;
using System.Data;
using System.Formats.Asn1;
using System.Windows;
using System.Windows.Threading;

namespace AsteroidGameWPF
{
    
    public partial class App : Application
    {
        private Game _model = null!;
        private MainWindow _view = null!;
        private AsteroidGameViewModel _viewModel = null!;
        private DispatcherTimer _timer = null!;
        

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(12);
            _timer.Tick += new EventHandler(Timer_Tick);

            _view = new MainWindow();
            _view.Loaded += View_Loaded;
            _view.Show();
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {  
            InitializeGame();
        }

        public void InitializeGame(Game? game = null)
        {
            if (game == null)
            {
                _model = new Game((int)_view.GameCanvas.ActualWidth, (int)_view.GameCanvas.ActualHeight);
            }
            else
            {
                _model = game;
            }

            _model.GameOverEvent += Model_GameOver;

            _viewModel = new AsteroidGameViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
            _view.DataContext = _viewModel;

            _viewModel.NewGameCommand?.RaiseCanExecuteChanged();
            _viewModel.LoadGameCommand?.RaiseCanExecuteChanged();
            _viewModel.SaveGameCommand?.RaiseCanExecuteChanged();

            _timer.Start();
        }


        private void Timer_Tick(object? sender, EventArgs e)
        {
            
           _model.Update();
            
        }

        private async void Model_GameOver(object? sender, EventArgs e)
        {
                
            await _viewModel.WaitForGameOverCompletionAsync();
            MessageBox.Show("Game Over! " + _viewModel.ElapsedTime);
        }

        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            InitializeGame();
            _model.StartGame();

        }

        private async void ViewModel_LoadGame(object? sender, EventArgs e)
        {
            IGamePersistence persistence = new FileGamePersistence();
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                Title = "Load Game"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    Game loadedGame = await persistence.LoadGameAsync(openFileDialog.FileName);
                    InitializeGame(loadedGame);
                    _model.AddTime(loadedGame.ElapsedTime);
                    _model.OnGameStateChanged();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while loading the game: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            IGamePersistence persistence = new FileGamePersistence();
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                Title = "Save Game"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    await persistence.SaveGameAsync(_model, saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving the game: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

       

        
    }

}
