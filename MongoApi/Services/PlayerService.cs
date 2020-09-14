using MongoApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace MongoApi.Services
{
    public class PlayerService
    {
        private readonly IMongoCollection<Player> playerList;

        public PlayerService(IPlayerDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            playerList = database.GetCollection<Player>(settings.PlayerCollectionName);
        }

        public List<Player> Get() =>
            playerList.Find(player => true).ToList();

        public Player Get(string id) =>
            playerList.Find<Player>(player => player.Id == id).FirstOrDefault();

        public Player Create(Player player)
        {
            playerList.InsertOne(player);
            return player;
        }

        public void Update(string id, Player updatedPlayer) =>
            playerList.ReplaceOne(player => player.Id == id, updatedPlayer);

        public void Remove(Player playerToBeDeleted) =>
            playerList.DeleteOne(player => player.Id == playerToBeDeleted.Id);

        public void Remove(string id) => 
            playerList.DeleteOne(player => player.Id == id);
    }
}