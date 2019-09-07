using CommunityAPI.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CommunityAPI.Data
{
    public class ModelContext : IdentityDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TechnologyScore> TechnologyScore { get; set; }
        public DbSet<Technology> Technology { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<UserTechnology> UserTechnology { get; set; }


        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TechnologyScore>()
                .HasAlternateKey(c => new { c.SourceUserID, c.TargetUserID, c.TechnologyID })
                .HasName("AK_SourceUserID_TargetUserID_TechnologyID");
            modelBuilder.Entity<UserTechnology>()
                .HasAlternateKey(c => new { c.TechnologyID, c.UserID })
                .HasName("AK_TechnologyID_UserID");
            modelBuilder.Entity<Technology>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
