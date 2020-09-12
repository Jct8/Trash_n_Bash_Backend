using Microsoft.EntityFrameworkCore;

namespace MySQLapi
{
    public class MatchContext : DbContext
    {
        public MatchContext(DbContextOptions<MatchContext> options)
            : base(options)
        {
        }

        public DbSet<Match> match { get; set; }
    }
}
