using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidGame.Model
{
    public class Asteroid
    {
        private int _positionX;
        private int _positionY;

        public static int Speed { get; set; } = 5;
        public static int Size { get; set; } = 30;

        public int PositionX
        {
            get { return _positionX; }
            set { _positionX = value; }
        }

        public int PositionY
        {
            get { return _positionY; }
            set { _positionY = value; }
        }

        public Asteroid(int X, int Y)
        {
            PositionX = X;
            PositionY = Y;
        }

        public void MoveDown()
        {
            PositionY += Speed;
        }
    }
}
