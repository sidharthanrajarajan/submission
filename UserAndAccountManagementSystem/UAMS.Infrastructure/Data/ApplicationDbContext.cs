using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UAMS.Domain.Entities;
using UAMS.Infrastructure.Identity;

namespace UAMS.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("training");
            // Relationships between ApplicationUser and RefreshTokens
            builder.Entity<RefreshToken>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(rt => rt.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
