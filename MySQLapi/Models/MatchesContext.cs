using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MySQLapi
{
    public class MatchContext : DbContext
    {
        public MatchContext(DbContextOptions<MatchContext> options)
            : base(options)
        {
        }

        public void HowManyGamesPlayed()
        {
            var totalGames = match.Count();
            System.Console.WriteLine("Total Games Played is: " + totalGames);
        }

        public void AverageScore()
        {
            var average = match.Average(x => x.score);
            System.Console.WriteLine("Average score over all games is: " + average);
        }

        public DbSet<Match> match { get; set; }
    }
}
