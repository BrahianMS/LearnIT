using LearnIT.Domain.Entities;
using LearnIT.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnIT.Infrastructure.Persistence
{
    public class LearnITDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public LearnITDbContext(DbContextOptions<LearnITDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LearnITDbContext).Assembly);

            modelBuilder.Entity<Course>()
                .HasQueryFilter(c => !c.IsDeleted);

            modelBuilder.Entity<Lesson>()
                .HasQueryFilter(l => !l.IsDeleted);
        }
    }
}
