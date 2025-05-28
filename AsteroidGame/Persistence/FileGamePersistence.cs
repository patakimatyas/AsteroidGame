using AsteroidGame.Model;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json; 
using System.Threading.Tasks;

namespace AsteroidGame.Persistence
{
    public class FileGamePersistence : IGamePersistence
    {
        public async Task SaveGameAsync(Game game, string filePath)
        {
            string json = JsonConvert.SerializeObject(game);
            await File.WriteAllTextAsync(filePath, json);
        }

        public async Task<Game> LoadGameAsync(string filePath)
        {
            try
            {
                string json = await File.ReadAllTextAsync(filePath);
                var game = JsonConvert.DeserializeObject<Game>(json);

                if (game == null)
                {
                    throw new InvalidDataException("Deserialization returned null.");
                }

                return game;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while loading the game.", ex);
            }
        }
       
    }
}
