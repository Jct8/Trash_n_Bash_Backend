using System;
using System.ComponentModel.DataAnnotations;

public class Player
{
    [Key]
    public int player_id { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public DateTime date_of_birth { get; set; }
    public string email { get; set; }
    public string nickname { get; set; }
    public bool opt_in { get; set; }
}