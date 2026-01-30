using Domain.Entities;
using Infraestructur.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Required
using Microsoft.EntityFrameworkCore;

namespace Infraestructur
{
    // Inherit from IdentityDbContext to enable Identity features
    // Pass <UserCredential, IdentityRole<Guid>, Guid>
    public class ApplicationDbContext : IdentityDbContext<UserCredential, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Keep your Domain sets
        public DbSet<User> Users { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }
        public DbSet<UserIdentity> UserIdentities { get; set; }
        // Note: You don't need DbSet<UserCredential> because IdentityDbContext handles it

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Always call the base first for Identity configuration
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // 1. Map Identity User to your 'user_credentials' table
            modelBuilder.Entity<UserCredential>(entity =>
            {
                entity.ToTable("user_credentials");
                // Maps Identity's internal "Id" to your SQL "users_id"
                entity.Property(e => e.Id).HasColumnName("users_id");
            });

            // 2. Map Domain User to your 'users' table
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
            });

            // 3. Define the 1:1 relationship between Domain and Identity
            modelBuilder.Entity<UserCredential>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<UserCredential>(c => c.Id);
        }
    }
}