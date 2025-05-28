using System.Collections.ObjectModel;
using AsteroidGame.Model;
using System.Windows.Input;
using System.Windows.Threading;


namespace AsteroidGameWPF.ViewModel
{
    public class AsteroidGameViewModel : ViewModelBase
    {
        private Game _model;

        private TaskCompletionSource<bool> _gameOverCompletionSource;

        public bool Paused => _model.Paused;
        public int SpaceshipTop => _model.ScreenHeight - Spaceship.Size;

        private DispatcherTimer _moveLeftTimer;
        private DispatcherTimer _moveRightTimer;

        public string ElapsedTime => _model.ElapsedTime.ToString(@"mm\:ss");

        public DelegateCommand MoveLeftCommand { get; private set; }
        public DelegateCommand MoveRightCommand { get; private set; }

        public DelegateCommand? NewGameCommand { get; private set; }
        public DelegateCommand? LoadGameCommand { get; private set; }
        public DelegateCommand? SaveGameCommand { get; private set; }
        public DelegateCommand? TogglePauseCommand { get; private set; }

        public DelegateCommand? KeyUpCommand { get; }

        public event EventHandler? NewGame;
        public event EventHandler? LoadGame;
        public event EventHandler? SaveGame;

        public SpaceshipViewModel Spaceship { get; private set; }
        public ObservableCollection<AsteroidViewModel> Asteroids { get; private set; }


        public AsteroidGameViewModel(Game model)
        {
            _model = model;
            _model.GameStateChangedEvent += new EventHandler(Model_GameStateChanged);
            _model.GameOverEvent += new EventHandler(Model_GameOver);
            _model.GamePauseToggledEvent += new EventHandler((sender, e) => OnPropertyChanged(nameof(Paused)));
            _gameOverCompletionSource = new TaskCompletionSource<bool>();

            Spaceship = new SpaceshipViewModel(_model.Spaceship);

            _moveLeftTimer = new DispatcherTimer();
            _moveLeftTimer.Interval = TimeSpan.FromMilliseconds(12);
            _moveLeftTimer.Tick += (sender, e) => _model.OnMoveLeft();

            _moveRightTimer = new DispatcherTimer();
            _moveRightTimer.Interval = TimeSpan.FromMilliseconds(12);
            _moveRightTimer.Tick += (sender, e) => _model.OnMoveRight();

            MoveLeftCommand = new DelegateCommand(param => _moveLeftTimer.Start());
            MoveRightCommand = new DelegateCommand(param => _moveRightTimer.Start());

            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            TogglePauseCommand = new DelegateCommand(param => _model.ToggleWatch());
            KeyUpCommand = new DelegateCommand(param => OnKeyUp());

            Asteroids = new ObservableCollection<AsteroidViewModel>();

        }

        private void Model_GameStateChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(ElapsedTime));
            OnPropertyChanged(nameof(Spaceship));

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
                }
            }

        }

        public void OnKeyUp()
        {
            _moveLeftTimer.Stop();
            _moveRightTimer.Stop();
        }

        public Task WaitForGameOverCompletionAsync()
        {
            _gameOverCompletionSource = new TaskCompletionSource<bool>();
            return _gameOverCompletionSource.Task;
        }

        private void Model_GameOver(object? sender, EventArgs e)
        {

            _moveLeftTimer.Stop();
            _moveRightTimer.Stop();
            _gameOverCompletionSource?.SetResult(true);
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
