using System;

namespace MongoApi.Models
{
    public class Match
    {
        public int level_number { get; set; }
        public double score { get; set; }
        public DateTime date { get; set; }
    }
    public class TopTenMatch
    {
        public string player_nickname { get; set; }
        public int level_number { get; set; }
        public double score { get; set; }
        public DateTime date { get; set; }
    }
    public class MatchDuration
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}