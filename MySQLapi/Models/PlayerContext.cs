using Microsoft.EntityFrameworkCore;

namespace MySQLapi
{
    public class PlayerContext : DbContext
    {
        public PlayerContext(DbContextOptions<PlayerContext> options)
            : base(options)
        {
        }

        public DbSet<Player> player { get; set; }
    }
}
