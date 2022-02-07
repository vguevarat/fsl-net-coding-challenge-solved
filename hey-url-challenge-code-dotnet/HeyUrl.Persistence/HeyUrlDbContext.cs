using HeyUrl.Entity;
using Microsoft.EntityFrameworkCore;

namespace HeyUrl.Persistence
{
    public class HeyUrlDbContext : DbContext
    {
        public HeyUrlDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Url> Urls { get; set; }
        public DbSet<Click> Clicks { get; set; }
        public DbSet<ShortUrl> ShortUrls { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Url>()
                .HasIndex(e => new { e.ShortUrl })
                .IsUnique(true);
        }
    }
}
