using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructur
{
    public class BusStopDbContext : DbContext
    {
        public BusStopDbContext(DbContextOptions<BusStopDbContext> options)
            : base(options)
        {
        }

        // DbSets remain the same
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<StopType> StopTypes { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This single line replaces all the manual entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BusStopDbContext).Assembly);

            // It's still good practice to call the base method, though ApplyConfigurationsFromAssembly 
            // often overrides default conventions.
            base.OnModelCreating(modelBuilder);
        }
    }
}

