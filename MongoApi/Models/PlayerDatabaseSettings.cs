

namespace MongoApi.Models
{
    public interface IPlayerDatabaseSettings
    {
        string PlayerCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
    public class PlayerDatabaseSettings : IPlayerDatabaseSettings
    {
        public string PlayerCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}