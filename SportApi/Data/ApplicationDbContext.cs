using Microsoft.EntityFrameworkCore;
using SportApi.Models;

namespace SportApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MatchResult> MatchResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<MatchResult>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MatchId).IsRequired(false);
                entity.Property(e => e.HomeTeam).IsRequired(false);
                entity.Property(e => e.AwayTeam).IsRequired(false);
                entity.Property(e => e.HomeScore);
                entity.Property(e => e.AwayScore);
                
            });
        }
    }
}
