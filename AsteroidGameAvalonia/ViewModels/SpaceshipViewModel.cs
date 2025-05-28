using System;
using System.ComponentModel;
using AsteroidGame.Model;

namespace AsteroidGameAvalonia.ViewModels
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
                   _spaceship.Position = value;
                   OnPropertyChanged(); 
            }
        }

        public int Size => Spaceship.Size;


    }
}
