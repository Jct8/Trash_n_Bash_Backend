using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoApi.Models
{
    public class Match
    {
        public int level_number { get; set; }
        public int score { get; set; }
        public DateTime date { get; set; }
    }

    public class MatchDuration
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}