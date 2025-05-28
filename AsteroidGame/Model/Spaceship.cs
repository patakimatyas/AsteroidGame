using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidGame.Model
{
    public class Spaceship
    {
        private int _position;

        public static int Size { get; set; } = 60;
        public static int Speed { get; set; } = 8;

        public event EventHandler? SpaceshipMovedEvent;

        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Spaceship(int ScreenWidth)
        {
            Position = ScreenWidth / 2;
        } 

        public void MoveLeft()
        {
            Position -= Speed;
            SpaceshipMovedEvent?.Invoke(this, EventArgs.Empty);
        }

        public void MoveRight()
        {
            Position += Speed;
            SpaceshipMovedEvent?.Invoke(this, EventArgs.Empty);

        }
    }

}
