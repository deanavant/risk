using Microsoft.EntityFrameworkCore;
 
namespace risk.Models
{
    public class RiskContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public RiskContext(DbContextOptions<RiskContext> options) : base(options) { }
        public DbSet<Player> player { get; set;}
        public DbSet<Territory> territory { get; set;}
        public DbSet<Game> game { get; set;}
    }
}
