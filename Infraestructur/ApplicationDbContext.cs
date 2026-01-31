// Infraestructur/ApplicationDbContext.cs
using Domain.Entities;
using Infraestructur.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<UserCredential, AppRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> DomainUsers { get; set; } // Tu tabla 'users'
    public DbSet<User> Users { get; set; }
    public DbSet<Route> Routes { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<CommentReaction> CommentReactions { get; set; }
    public DbSet<UserIdentity> UserIdentities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. PRIMERO: Configuración base de Identity
        base.OnModelCreating(modelBuilder);

        // 2. Mapear 'UserCredential' a 'user_credentials'
        modelBuilder.Entity<UserCredential>(entity => {
            entity.ToTable("user_credentials");
            entity.Property(e => e.Id).HasColumnName("users_id");
        });

        // 3. Mapear 'AppRole' a tu tabla 'roles'
        modelBuilder.Entity<AppRole>(entity => {
            entity.ToTable("roles");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");

            // Ignoramos columnas innecesarias de Identity si tu tabla no las tiene
            entity.Ignore(r => r.ConcurrencyStamp);
            entity.Ignore(r => r.NormalizedName);
        });

        // 4. Mapear la tabla intermedia IdentityUserRole a 'roles_has_users'
        modelBuilder.Entity<IdentityUserRole<Guid>>(entity => {
            entity.ToTable("roles_has_users");
            entity.HasKey(r => new { r.UserId, r.RoleId });
            entity.Property(e => e.UserId).HasColumnName("users_id");
            entity.Property(e => e.RoleId).HasColumnName("roles_id");
        });

        // 5. Tu tabla 'users' de dominio
        modelBuilder.Entity<User>(entity => {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
        });

        // Otros mapeos de tu ensamblado
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}