using System;
using System.ComponentModel.DataAnnotations;

public class Match
{
    [Key]
    public int match_id { get; set; }
    public int player_id { get; set; }
    public int level_number { get; set; }
    public int score { get; set; }
    public DateTime date { get; set; }
}

public class MatchDuration
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set;}
}