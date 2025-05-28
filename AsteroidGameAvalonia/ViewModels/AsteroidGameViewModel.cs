using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AsteroidGame.Model;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Threading;

namespace AsteroidGameAvalonia.ViewModels
{
    public class AsteroidGameViewModel : ViewModelBase
    {
        private Game _model;

        private TaskCompletionSource<bool> _gameOverCompletionSource;

        public bool Paused => _model.Paused;
        public bool SaveGameEnabled => Paused && (_asteroids.Count>0);
        public int GameHeight => _model.ScreenHeight;
        public int GameWidth => _model.ScreenWidth;
        public int SpaceshipTop => GameHeight - Spaceship.Size; 

        public string ElapsedTime => _model.ElapsedTime.ToString(@"mm\:ss");

        public RelayCommand MoveLeftCommand { get; private set; }
        public RelayCommand MoveRightCommand { get; private set; }

        public RelayCommand? NewGameCommand { get; private set; }
        public RelayCommand? LoadGameCommand { get; private set; }
        public RelayCommand? SaveGameCommand { get; private set; }
        public RelayCommand? TogglePauseCommand { get; private set; }

        public event EventHandler? NewGame;
        public event EventHandler? LoadGame;
        public event EventHandler? SaveGame;
        
        public SpaceshipViewModel Spaceship { get; private set; }
        private ObservableCollection<AsteroidViewModel> _asteroids;
        public ObservableCollection<AsteroidViewModel> Asteroids
        {
            get => _asteroids;
            set
            {
                if (_asteroids != value)
                {
                    _asteroids = value;
                    OnPropertyChanged();

                }
            }
        }


        public AsteroidGameViewModel(Game model)
        {
            _model = model;
            _model.GameStateChangedEvent += new EventHandler(Model_GameStateChanged);
            _model.GamePauseToggledEvent += new EventHandler((sender, e) => OnPropertyChanged(nameof(Paused)));
            _gameOverCompletionSource = new TaskCompletionSource<bool>();

            Spaceship = new SpaceshipViewModel(_model.Spaceship);

            MoveLeftCommand = new RelayCommand(MoveLeft);
            MoveRightCommand = new RelayCommand(MoveRight);
            NewGameCommand = new RelayCommand(OnNewGame);
            LoadGameCommand = new RelayCommand(OnLoadGame);
            SaveGameCommand = new RelayCommand(OnSaveGame);
            TogglePauseCommand = new RelayCommand(PauseGame);

            _asteroids = new ObservableCollection<AsteroidViewModel>();
            Asteroids = new ObservableCollection<AsteroidViewModel>();
        }

        private void MoveLeft()
        {
            _model.OnMoveLeft();
        }
        private void MoveRight()
        {
            _model.OnMoveRight();
        }
        private void PauseGame()
        {
            _model.ToggleWatch();
            OnPropertyChanged(nameof(SaveGameEnabled));

        }

        private void Model_GameStateChanged(object? sender, EventArgs e)
        {

            
            Spaceship.Position = _model.Spaceship.Position;
            
            for (int i = 0; i < Asteroids.Count; i++)
            {
                if (Asteroids[i].PositionY > _model.ScreenHeight)
                {
                    Asteroids.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
            for (int i = 0; i < _model.Asteroids.Count; i++)
            {

                if (i < Asteroids.Count)
                {
                    Asteroids[i].PositionX = _model.Asteroids[i].PositionX;
                    Asteroids[i].PositionY = _model.Asteroids[i].PositionY;
                }
                else
                {
                    Asteroids.Add(new AsteroidViewModel(_model.Asteroids[i]));
                    OnPropertyChanged(nameof(SaveGameEnabled));


                }
            }

            OnPropertyChanged(nameof(ElapsedTime));
            

        }


        public Task WaitForGameOverCompletionAsync()
        {
            _gameOverCompletionSource = new TaskCompletionSource<bool>();
            return _gameOverCompletionSource.Task;
        }

        private void OnNewGame()
        { 
            NewGame?.Invoke(this, EventArgs.Empty); 
        }

        private void OnLoadGame()
        {
            LoadGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }


    }
}
