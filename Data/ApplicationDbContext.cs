// Data/ApplicationDbContext.cs
using EmployeeDirectory.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDirectory.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ─── shrink all Identity key/index columns to 83 chars ───

            // Users table PK
            builder.Entity<IdentityUser>(e =>
                e.Property(u => u.Id).HasMaxLength(83));

            // Roles table PK & name/index
            builder.Entity<IdentityRole>(e =>
            {
                e.Property(r => r.Id).HasMaxLength(83);
                e.Property(r => r.Name).HasMaxLength(83);
                e.Property(r => r.NormalizedName).HasMaxLength(83);
            });

            // UserLogins composite PK (LoginProvider + ProviderKey)
            builder.Entity<IdentityUserLogin<string>>(e =>
            {
                e.Property(l => l.LoginProvider).HasMaxLength(83);
                e.Property(l => l.ProviderKey).HasMaxLength(83);
                e.Property(l => l.UserId).HasMaxLength(83);
            });

            // UserTokens composite PK (UserId + LoginProvider + Name)
            builder.Entity<IdentityUserToken<string>>(e =>
            {
                e.Property(t => t.UserId).HasMaxLength(83);
                e.Property(t => t.LoginProvider).HasMaxLength(83);
                e.Property(t => t.Name).HasMaxLength(83);
            });

            // UserRoles composite PK (UserId + RoleId)
            builder.Entity<IdentityUserRole<string>>(e =>
            {
                e.Property(r => r.UserId).HasMaxLength(83);
                e.Property(r => r.RoleId).HasMaxLength(83);
            });

            // Claims tables (just to be safe)
            builder.Entity<IdentityUserClaim<string>>(e =>
                e.Property(c => c.Id).HasMaxLength(83));
            builder.Entity<IdentityRoleClaim<string>>(e =>
                e.Property(c => c.Id).HasMaxLength(83));
        }
    }
}
