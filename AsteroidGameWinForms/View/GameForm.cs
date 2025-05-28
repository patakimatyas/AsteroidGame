using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AsteroidGame.Model;
using AsteroidGame.Persistence;

namespace AsteroidGameWinForms
{
    public partial class GameForm : Form
    {
        private Game game;
        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Timer moveLeftTimer;
        private System.Windows.Forms.Timer moveRightTimer;
        private Bitmap asteroidImage;
        private Bitmap spaceshipImage;


        public GameForm()
        {
            InitializeComponent();

            KeyPreview = true;
            DoubleBuffered = true;

            NewGame.Enter += (s, e) => ActiveControl = null;
            SaveGame.Enter += (s, e) => ActiveControl = null;
            LoadGame.Enter += (s, e) => ActiveControl = null;

            game = new Game(ClientSize.Width - panel.Width, ClientSize.Height);
            Resize += new EventHandler(GameForm_Resize);

            asteroidImage = new Bitmap("Resources/asteroid.png");
            spaceshipImage = new Bitmap("Resources/spaceship.png");

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 10;
            gameTimer.Tick += GameLoop;

            moveLeftTimer = new System.Windows.Forms.Timer();
            moveLeftTimer.Interval = 10;
            moveLeftTimer.Tick += (s, e) => game.OnMoveLeft();

            moveRightTimer = new System.Windows.Forms.Timer();
            moveRightTimer.Interval = 10;
            moveRightTimer.Tick += (s, e) => game.OnMoveRight();

            game.GameStateChangedEvent += OnGameStateChanged;
            game.GameOverEvent += OnGameOver;


        }

        #region Resize
        private void GameForm_Resize(object? sender, EventArgs e)
        {
            game.ScreenWidth = this.ClientSize.Width - panel.Width;
            game.ScreenHeight = this.ClientSize.Height;

            game.Spaceship.Position = game.ScreenWidth / 2;

            this.Invalidate();
        }
        #endregion

        #region GameLoop
        private void GameLoop(object? sender, EventArgs e)
        {
            game.Update();
            TimeLabel.Text = string.Format("{0:00}:{1:00}", game.ElapsedTime.Minutes, game.ElapsedTime.Seconds);
        }

        private void OnGameStateChanged(object? sender, EventArgs e)
        {
            Invalidate();
        }

        private void OnGameOver(object? sender, EventArgs e)
        {
            moveLeftTimer.Stop();
            moveRightTimer.Stop();
            gameTimer.Stop();
            game.ToggleWatch();

            string formattedTime = string.Format("{0:D2}:{1:D2}", game.ElapsedTime.Minutes, game.ElapsedTime.Seconds);
            MessageBox.Show("Game Over! Time: " + formattedTime);

            ToggleMenu();
            SaveGame.Enabled = false;

        }

        #endregion

        #region NewGame
        private void NewGame_Click(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            KeyPreview = true;

            game = new Game(ClientSize.Width - panel.Width, ClientSize.Height);
            gameTimer.Start();
            game.GameOverEvent += OnGameOver;
            game.GameStateChangedEvent += OnGameStateChanged;
            game.ToggleWatch();

            SaveGame.Enabled = true;
            ToggleMenu();
            pausedLabel.Visible = false;

            game.Started = true;
            Invalidate();
        }
        #endregion

        #region SaveGame
        private async void SaveGame_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON files (*.json)|*.json";
            saveFileDialog.InitialDirectory = "SavedGames";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                IGamePersistence gamePersistence = new FileGamePersistence();

                await gamePersistence.SaveGameAsync(game, filePath);

                MessageBox.Show("Game saved successfully!");
            }
        }
        #endregion

        #region LoadGame
        private async void LoadGame_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json";
            openFileDialog.InitialDirectory = "SavedGames";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                IGamePersistence gamePersistence = new FileGamePersistence();

                game = await gamePersistence.LoadGameAsync(filePath);
                gameTimer.Start();

                game.GameOverEvent += OnGameOver;
                game.GameStateChangedEvent += OnGameStateChanged;

                game.AddTime(game.ElapsedTime);

                pausedLabel.Visible = true;
                game.Started = true;
                SaveGame.Enabled = true;

                Invalidate();

                MessageBox.Show("Game loaded successfully!");
            }
        }
        #endregion 

        #region HandleKeys
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (gameTimer.Enabled && e.KeyCode == Keys.Space)
            {
                moveLeftTimer.Enabled = false;
                moveRightTimer.Enabled = false;
                TogglePause();
            }
            else if (!pausedLabel.Visible && e.KeyCode == Keys.Left && !moveLeftTimer.Enabled)
            {
                moveLeftTimer.Start();
            }
            else if (!pausedLabel.Visible && e.KeyCode == Keys.Right && !moveRightTimer.Enabled)
            {
                moveRightTimer.Start();
            }

        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.KeyCode == Keys.Left)
            {
                moveLeftTimer.Stop();
            }
            else if (e.KeyCode == Keys.Right)
            {
                moveRightTimer.Stop();
            }
        }

        private void ToggleMenu()
        {
            NewGame.Enabled = !NewGame.Enabled;
            SaveGame.Enabled = !SaveGame.Enabled;
            LoadGame.Enabled = !LoadGame.Enabled;
        }
        private void TogglePause()
        {
            if (game.Started)
            {
                pausedLabel.Visible = !pausedLabel.Visible;

                ToggleMenu();
                game.ToggleWatch();
            }
        }
        #endregion

        #region OnPaint
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            g.DrawImage(spaceshipImage, game.Spaceship.Position, this.ClientSize.Height - Spaceship.Size - 10, Spaceship.Size, Spaceship.Size);

            foreach (var asteroid in game.Asteroids)
            {
                g.DrawImage(asteroidImage, asteroid.PositionX, asteroid.PositionY, Asteroid.Size, Asteroid.Size);
            }

            if (!game.Started)
            {
                string startText = "Press 'New Game' to start";
                Font font = new Font("Arial", 24, FontStyle.Bold);
                SizeF textSize = g.MeasureString(startText, font);

                float x = (this.ClientSize.Width - panel.Width - textSize.Width) / 2;
                float y = (this.ClientSize.Height - textSize.Height) / 2;
                g.DrawString(startText, font, Brushes.White, x, y);
            }

        }
        #endregion






       
    }
}
