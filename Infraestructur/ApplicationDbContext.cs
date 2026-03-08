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

            // 1. Primary Key
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("users_id");

            // 2. Map Identity properties to your NEW snake_case columns
            entity.Property(u => u.UserName).HasColumnName("user_name");
            entity.Property(u => u.NormalizedUserName).HasColumnName("normalized_user_name");
            entity.Property(u => u.Email).HasColumnName("email");
            entity.Property(u => u.NormalizedEmail).HasColumnName("normalized_email");
            entity.Property(u => u.EmailConfirmed).HasColumnName("email_confirmed");
            entity.Property(u => u.PasswordHash).HasColumnName("password_hash");
            entity.Property(u => u.SecurityStamp).HasColumnName("security_stamp");
            entity.Property(u => u.ConcurrencyStamp).HasColumnName("concurrency_stamp");
            entity.Property(u => u.PhoneNumber).HasColumnName("phone_number");
            entity.Property(u => u.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
            entity.Property(u => u.TwoFactorEnabled).HasColumnName("two_factor_enabled");
            entity.Property(u => u.LockoutEnd).HasColumnName("lockout_end");
            entity.Property(u => u.LockoutEnabled).HasColumnName("lockout_enabled");
            entity.Property(u => u.AccessFailedCount).HasColumnName("access_failed_count");

            // 3. Custom properties
            entity.Property(u => u.RefreshToken).HasColumnName("refresh_token");

            // 4. Relationship
            entity.HasOne(uc => uc.User)
                  .WithOne()
                  .HasForeignKey<UserCredential>(uc => uc.Id);
        });

        // 3. Mapear 'AppRole' a tu tabla 'roles'
        modelBuilder.Entity<AppRole>(entity => {
            entity.ToTable("roles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");

            // FIX: Remove entity.Ignore and map the property to the column
            entity.Property(e => e.NormalizedName).HasColumnName("normalized_name");

            // You can keep ignoring ConcurrencyStamp if your table doesn't have it
            entity.Ignore(r => r.ConcurrencyStamp);
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

            //Tell EF not to map the domain 'Role' entity to the database
            entity.Ignore(e => e.Roles);
        });

        // Otros mapeos de tu ensamblado
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}