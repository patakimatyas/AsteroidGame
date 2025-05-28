using AsteroidGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; 


namespace AsteroidGame.Persistence
{
    public interface IGamePersistence
    {
        Task SaveGameAsync(Game game, string filePath);
        Task<Game> LoadGameAsync(string filePath);
    }
}
