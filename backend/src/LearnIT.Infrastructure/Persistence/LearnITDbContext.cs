using LearnIT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnIT.Infrastructure.Persistence
{
    public class LearnITDbContext : DbContext
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
