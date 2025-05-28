using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsteroidGame.Model;   

namespace AsteroidGameAvalonia.ViewModels
{
    public class AsteroidViewModel : ViewModelBase
    {
        private readonly Asteroid _asteroid;
        

        public int PositionX
        {
            get => _asteroid.PositionX;
            set
            {
                _asteroid.PositionX = value;
                OnPropertyChanged();
            }
        }
        public int PositionY
        {
            get => _asteroid.PositionY;
            set
            {
                _asteroid.PositionY = value;
                OnPropertyChanged();
            }
        }

        public int Size => Asteroid.Size;
        

        public AsteroidViewModel(Asteroid a)
        {
            _asteroid = a;
        }

    }
}
