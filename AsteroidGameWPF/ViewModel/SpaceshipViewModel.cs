using System;
using System.ComponentModel;
using AsteroidGame.Model;

namespace AsteroidGameWPF.ViewModel
{
    public class SpaceshipViewModel : ViewModelBase
    {
        private readonly Spaceship _spaceship;

        public SpaceshipViewModel(Spaceship spaceship)
        {
            _spaceship = spaceship;
        }

        public int Position
        {
            get => _spaceship.Position;
            set
            {
                if (_spaceship.Position != value)
                {
                    _spaceship.Position = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Size => Spaceship.Size;


    }
}
