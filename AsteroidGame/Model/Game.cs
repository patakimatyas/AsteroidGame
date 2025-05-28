using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace AsteroidGame.Model
{
    public class Game
    {
        public Spaceship Spaceship { get; set; }
        public List<Asteroid> Asteroids { get; set; }
        public bool Started { get; set; }
        public bool Paused { get; set; }
        public bool GameOver { get; set; }

        private TimeSpan _savedTime;
        private Stopwatch _gameTime;
        public TimeSpan ElapsedTime{ get; set; }

        private Random _random;
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        private int _minSpawnRate;
        private int _asteroidSpawnRate;
        private int _tickCounter;
        private int _spawnRateDecreaseInterval;

        public event EventHandler? GameOverEvent;
        public event EventHandler? GameStateChangedEvent;
        public event EventHandler? GamePauseToggledEvent;


        public Game(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;

            Asteroids = new List<Asteroid>();
            Spaceship = new Spaceship(width);
            Spaceship.SpaceshipMovedEvent += new EventHandler((sender, e) => GameStateChangedEvent?.Invoke(this, EventArgs.Empty));
            _gameTime = new Stopwatch();

            Started = false;
            GameOver = false;
            Paused = true;

            _random = new Random();

            _asteroidSpawnRate = 40;
            _spawnRateDecreaseInterval = 80;
            _minSpawnRate = 5;
            _tickCounter = 0;

        }
        public void AddTime(TimeSpan time)
        {
            _savedTime = time;
        }

        #region MoveEvents
        public void OnMoveLeft()
        {
            if (Spaceship.Position > Spaceship.Speed && _gameTime.IsRunning)
            {
                Spaceship.MoveLeft();
            }
        }
        public void OnMoveRight()
        {
            if (Spaceship.Position < (ScreenWidth - Spaceship.Speed - Spaceship.Size) && _gameTime.IsRunning)
            {
                Spaceship.MoveRight();
            }
        }
        #endregion

        public void StartGame()
        {
            _gameTime.Start();
            Started = true;
            GameOver = false;
            Paused = false;
            GamePauseToggledEvent?.Invoke(this, EventArgs.Empty);
        }

        public void ToggleWatch()
        {
            if (!GameOver)
            {
                Paused = !Paused;
                GamePauseToggledEvent?.Invoke(this, EventArgs.Empty);
                if (_gameTime.IsRunning) { _gameTime.Stop(); }
                else { _gameTime.Start(); }
            }
        }

      

        #region Update
        public void Update()
        {
            if (_gameTime.IsRunning)
            {
                _tickCounter++;

                CheckCollisions();

                IncreaseSpawnRate();

                MoveAsteroids();

                ElapsedTime = _savedTime + _gameTime.Elapsed;

                OnGameStateChanged();
            }

        }

        public void OnGameStateChanged()
        {
            GameStateChangedEvent?.Invoke(this, EventArgs.Empty);
        }

        private void MoveAsteroids()
        {
            if (_tickCounter % _asteroidSpawnRate == 0)
            {
                int startX = _random.Next(0, ScreenWidth - Asteroid.Size);
                Asteroids.Add(new Asteroid(startX, 0));
            }

            foreach (var asteroid in Asteroids)
            {
                asteroid.MoveDown();
            }

            Asteroids.RemoveAll(a => a.PositionY > ScreenHeight);
        }

        private void IncreaseSpawnRate()
        {
            if (_asteroidSpawnRate > _minSpawnRate && _tickCounter % _spawnRateDecreaseInterval == 0)
            {
                _asteroidSpawnRate--;
            }
        }

        private void CheckCollisions()
        {
            foreach (Asteroid asteroid in Asteroids)
            {
                if ((Spaceship.Position + 20 < asteroid.PositionX + Asteroid.Size &&
                    ScreenHeight - Spaceship.Size < asteroid.PositionY + Asteroid.Size) &&
                    Spaceship.Position + Spaceship.Size - 20 > asteroid.PositionX &&
                    ScreenHeight - 10 > asteroid.PositionY)
                {
                    OnGameOver();
                    break;
                }
            }
        }

        public void OnGameOver()
        {
            GameOver = true;
            Paused = true;
            _gameTime.Stop();
            GameOverEvent?.Invoke(this, EventArgs.Empty);
            GamePauseToggledEvent?.Invoke(this, EventArgs.Empty);
        }



        #endregion
    }
}

