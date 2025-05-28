using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsteroidGame.Model;

namespace AsteroidGameWPF.ViewModel
{
    public class AsteroidViewModel : ViewModelBase
    {
        private readonly Asteroid _asteroid;

        public int Size => Asteroid.Size;

        public int PositionX
        {
            get => _asteroid.PositionX;
            set
            {
                _asteroid.PositionX = value;
                OnPropertyChanged(nameof(PositionX));
            }
        }

        public int PositionY
        {
            get => _asteroid.PositionY;
            set
            {
                _asteroid.PositionY = value;
                OnPropertyChanged(nameof(PositionY));
            }
        }

        public AsteroidViewModel(Asteroid asteroid)
        {
            _asteroid = asteroid;
        }

    }
}
