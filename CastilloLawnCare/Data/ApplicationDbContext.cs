using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CastilloLawnCare.Models;

namespace CastilloLawnCare.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .Property(x => x.FirstName)
                .HasMaxLength(250);

            builder.Entity<User>()
                .Property(x => x.LastName)
                .HasMaxLength(250);

            builder.Entity<User>()
                .Property(x => x.City)
                .HasMaxLength(250);

            builder.Entity<User>()
                .Property(x => x.State)
                .HasMaxLength(2);

            builder.Entity<User>()
                .Property(x => x.Address)
                .HasMaxLength(250);
        }
    }
}