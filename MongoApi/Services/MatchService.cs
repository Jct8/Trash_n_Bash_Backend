using MongoApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace MongoApi.Services
{
    public class MatchService
    {
        private readonly IMongoCollection<Player> playerList;

        public MatchService(IPlayerDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            playerList = database.GetCollection<Player>(settings.PlayerCollectionName);
        }

        public List<Match> GetTopTenMatches(MatchDuration duration)
        {
            var filter = Builders<Player>.Filter
                .Where(x => x.matches.All(m => m.date >= duration.FromDate & m.date <= duration.ToDate));
           var filterBson = Builders<BsonDocument>.Filter
               .Where(x => x["matches"]["date"] >=  duration.FromDate & x["matches"]["date"] <= duration.ToDate);
            var documents = playerList
                .Aggregate()
                .Unwind(x => x.matches)
                .Match(filterBson)
                .SortByDescending(x => x["matches"]["score"])
                .Limit(10)
                .ToList();

            List<Match> topTenMatches = new List<Match>();
            foreach (var item in documents)
            {
                Match match = new Match();
                match.score = item["matches"]["score"].ToDouble();
                match.level_number = item["matches"]["level_number"].ToInt32();
                match.date = item["matches"]["date"].ToUniversalTime();
                topTenMatches.Add(match);
            }
            return topTenMatches;
        }

        public List<Match> GetAllMatchesFromPlayer(string playerId)
        {
            var filter = Builders<Player>.Filter.Where(player => player.Id == playerId);
            var matches = playerList.Find<Player>(filter).First().matches;
            return matches;
        }

        public Player CreateMatch(string playerId, Match match)
        {
            var player = playerList.Find<Player>(player => player.Id == playerId).FirstOrDefault();

            var filter = Builders<Player>.Filter.Where(player => player.Id == playerId);
            var update = Builders<Player>.Update.Push("matches", match);
            playerList.FindOneAndUpdate(filter, update);
            return player;
        }

        public void HowManyGamesPlayed()
        {
            var filter = Builders<Player>.Filter
                .Exists(x => x.matches);
            var totalGames = playerList
                .Aggregate()
                .Match(filter)
                .Unwind(x => x.matches).ToList()
                .Count();
            System.Console.WriteLine("Total Games Played is: " + totalGames);
        }

        public void AverageScore()
        {
            var filter = Builders<Player>.Filter
                .Exists(x => x.matches);
            var average = playerList
                .Aggregate()
                .Match(filter)
                .Unwind(x => x.matches)
                .ToList()
                .Average(x => x["matches"]["score"].AsDouble);
            System.Console.WriteLine("Average score over all games is: " + average);
        }

        // Not Used
        public void Update(string playerId, Match match)
        {
            var filter = Builders<Player>.Filter.Where(player => player.Id == playerId);
            var update = Builders<Player>.Update.Set("matches", match);
            playerList.FindOneAndUpdate(filter, update);
        }

        public void RemoveAllMatches(Player playerToBeDeleted)
        {
            var filter = Builders<Player>.Filter.Where(player => player.Id == playerToBeDeleted.Id);
            var update = Builders<Player>.Update.Unset(player => player.matches);
            playerList.FindOneAndUpdate(filter, update);
        }

        public void RemoveOneMatch(string playerId, Match match)
        {
            var filter = Builders<Player>.Filter.Where(player => player.Id == playerId);
            var update = Builders<Player>.Update.Pull("matches", match);
            playerList.FindOneAndUpdate(filter, update);
        }

    }
}