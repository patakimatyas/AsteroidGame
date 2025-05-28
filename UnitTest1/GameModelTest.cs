using AsteroidGame.Model;

namespace UnitTest
{
    [TestClass]
    public class GameModelTest
    {
        private Game _game = null!;

        [TestInitialize]
        public void Setup()
        {
            _game = new Game(800, 600);
            _game.ToggleWatch();
        }

        [TestMethod]
        public void InitializesCorrectly()
        {
            Assert.IsFalse(_game.Started);
            Assert.AreEqual(0, _game.Asteroids.Count);
            Assert.AreEqual(400, _game.Spaceship.Position);

        }

        [TestMethod]
        public void MovesLeft()
        {
            int initialPosition = _game.Spaceship.Position;

            _game.OnMoveLeft();

            Assert.IsTrue(_game.Spaceship.Position < initialPosition);
        }

        [TestMethod]
        public void MovesRight()
        {
            int initialPosition = _game.Spaceship.Position;

            _game.OnMoveRight();

            Assert.IsTrue(_game.Spaceship.Position > initialPosition);
        }

        [TestMethod]
        public void CannotMoveBeyondLeftBoundary()
        {
            _game.Spaceship.Position = 0;

            _game.OnMoveLeft();

            Assert.AreEqual(0, _game.Spaceship.Position);
        }

        [TestMethod]
        public void CannotMoveBeyondRightBoundary()
        {
            _game.Spaceship.Position = _game.ScreenWidth - Spaceship.Size;

            _game.OnMoveRight();

            Assert.AreEqual(_game.ScreenWidth - Spaceship.Size, _game.Spaceship.Position);
        }

        [TestMethod]
        public void AsteroidSpawn()
        {
            // Mivel a spawnrate 40 ezért 40 "tick" azaz update után generálódik 1 aszteroida
            for (int i = 0; i < 40; i++)
            {
                _game.Update();

            }

            Assert.IsTrue(_game.Asteroids.Count > 0);
        }

        [TestMethod]
        public void GameEndsWithCollision()
        {
            bool gameOverEventTriggered = false;

            _game.GameOverEvent += (sender, e) => gameOverEventTriggered = true;

            _game.Asteroids.Add(new Asteroid(_game.Spaceship.Position, 600 - Spaceship.Size));

            _game.Update();

            Assert.IsTrue(gameOverEventTriggered);
        }

        public void CannotMoveWhenPaused()
        {
            int initialPosition = _game.Spaceship.Position;

            _game.ToggleWatch();
            _game.OnMoveLeft();
            _game.OnMoveRight();

            Assert.AreEqual(initialPosition, _game.Spaceship.Position);
        }
    }
}
