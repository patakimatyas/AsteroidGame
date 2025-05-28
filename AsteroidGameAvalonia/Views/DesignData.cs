using AsteroidGameAvalonia.ViewModels;
using System;
using AsteroidGame.Model;
using AsteroidGame.Persistence;

namespace AsteroidGameAvalonia.Views
{
    public static class DesignData
    {
        public static AsteroidGameViewModel ViewModel
        {
            get
            {
                var model = new Game(800, 600);
                return new AsteroidGameViewModel(model);
            }
        }
    }
}
