namespace AsteroidGameWinForms
{
    partial class GameForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            NewGame = new Button();
            panel = new Panel();
            pausedLabel = new Label();
            TimeLabel = new Label();
            LoadGame = new Button();
            SaveGame = new Button();
            panel.SuspendLayout();
            SuspendLayout();
            // 
            // NewGame
            // 
            NewGame.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            NewGame.AutoSize = true;
            NewGame.BackColor = Color.FromArgb(224, 224, 224);
            NewGame.Font = new Font("Showcard Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            NewGame.Location = new Point(7, 11);
            NewGame.Name = "NewGame";
            NewGame.Size = new Size(154, 50);
            NewGame.TabIndex = 0;
            NewGame.TabStop = false;
            NewGame.Text = "New Game";
            NewGame.UseVisualStyleBackColor = false;
            NewGame.Click += NewGame_Click;
            // 
            // panel
            // 
            panel.AutoSize = true;
            panel.BackColor = Color.Silver;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Controls.Add(pausedLabel);
            panel.Controls.Add(TimeLabel);
            panel.Controls.Add(NewGame);
            panel.Controls.Add(LoadGame);
            panel.Controls.Add(SaveGame);
            panel.Dock = DockStyle.Right;
            panel.Location = new Point(715, 0);
            panel.Name = "panel";
            panel.Size = new Size(170, 541);
            panel.TabIndex = 1;
            // 
            // pausedLabel
            // 
            pausedLabel.AutoSize = true;
            pausedLabel.BackColor = Color.Transparent;
            pausedLabel.FlatStyle = FlatStyle.Popup;
            pausedLabel.Font = new Font("Arial", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            pausedLabel.ForeColor = Color.Black;
            pausedLabel.Location = new Point(7, 247);
            pausedLabel.Margin = new Padding(3);
            pausedLabel.MaximumSize = new Size(154, 50);
            pausedLabel.MinimumSize = new Size(154, 50);
            pausedLabel.Name = "pausedLabel";
            pausedLabel.RightToLeft = RightToLeft.No;
            pausedLabel.Size = new Size(154, 50);
            pausedLabel.TabIndex = 4;
            pausedLabel.Text = "PAUSED";
            pausedLabel.TextAlign = ContentAlignment.MiddleCenter;
            pausedLabel.Visible = false;
            // 
            // TimeLabel
            // 
            TimeLabel.AutoSize = true;
            TimeLabel.BackColor = Color.Transparent;
            TimeLabel.FlatStyle = FlatStyle.Popup;
            TimeLabel.Font = new Font("Arial", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TimeLabel.ForeColor = Color.Black;
            TimeLabel.Location = new Point(7, 191);
            TimeLabel.Margin = new Padding(3);
            TimeLabel.MaximumSize = new Size(154, 50);
            TimeLabel.MinimumSize = new Size(154, 50);
            TimeLabel.Name = "TimeLabel";
            TimeLabel.RightToLeft = RightToLeft.No;
            TimeLabel.Size = new Size(154, 50);
            TimeLabel.TabIndex = 3;
            TimeLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LoadGame
            // 
            LoadGame.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LoadGame.AutoSize = true;
            LoadGame.BackColor = Color.FromArgb(224, 224, 224);
            LoadGame.Font = new Font("Showcard Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LoadGame.Location = new Point(7, 123);
            LoadGame.Name = "LoadGame";
            LoadGame.Size = new Size(154, 50);
            LoadGame.TabIndex = 1;
            LoadGame.TabStop = false;
            LoadGame.Text = "Load Game";
            LoadGame.UseVisualStyleBackColor = false;
            LoadGame.Click += LoadGame_Click;
            // 
            // SaveGame
            // 
            SaveGame.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            SaveGame.AutoSize = true;
            SaveGame.BackColor = Color.FromArgb(224, 224, 224);
            SaveGame.Font = new Font("Showcard Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SaveGame.Location = new Point(7, 67);
            SaveGame.Name = "SaveGame";
            SaveGame.Size = new Size(154, 50);
            SaveGame.TabIndex = 2;
            SaveGame.TabStop = false;
            SaveGame.Text = "Save Game";
            SaveGame.UseVisualStyleBackColor = false;
            SaveGame.Click += SaveGame_Click;
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(885, 541);
            Controls.Add(panel);
            DoubleBuffered = true;
            KeyPreview = true;
            Name = "GameForm";
            Text = "Asteroid Game";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button NewGame;
        private Panel panel;
        private Button SaveGame;
        private Button LoadGame;
        private Label TimeLabel;
        public Label pausedLabel;
    }
}
