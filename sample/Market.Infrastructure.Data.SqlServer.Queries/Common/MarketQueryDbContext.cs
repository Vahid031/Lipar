using Lipar.Infrastructure.Data.SqlServer.Queries;
using Market.Infrastructure.Data.SqlServer.Queries.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.Infrastructure.Data.SqlServerQuery.Common;

public partial class MarketQueryDbContext : BaseQueryDbContext
{
    public MarketQueryDbContext(DbContextOptions<MarketQueryDbContext> options) : base(options)
    {
    }
    
public virtual DbSet<EntityChangesInterception> EntityChangesInterceptions { get; set; }
public virtual DbSet<InBoxEvent> InBoxEvents { get; set; }
public virtual DbSet<OutBoxEvent> OutBoxEvents { get; set; }
public virtual DbSet<Product> Products { get; set; }
public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
public virtual DbSet<Role> Roles { get; set; }
public virtual DbSet<RoleClaim> RoleClaims { get; set; }
public virtual DbSet<User> Users { get; set; }
public virtual DbSet<UserClaim> UserClaims { get; set; }
public virtual DbSet<UserLogin> UserLogins { get; set; }
public virtual DbSet<UserRole> UserRoles { get; set; }
public virtual DbSet<UserToken> UserTokens { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
        
        modelBuilder.Entity<EntityChangesInterception>(entity =>
        {
            entity.HasKey(e => e.Id)
            .IsClustered(false);
            
            entity.ToTable("_EntityChangesInterceptions");
            
            entity.HasIndex(e => e.Date, "IX__EntityChangesInterceptions_Date")
            .IsUnique()
            .IsClustered();
            
            entity.Property(e => e.Id).ValueGeneratedNever();
            
            entity.Property(e => e.EntityType).HasMaxLength(50);
            
            entity.Property(e => e.State).HasMaxLength(10);
        });
        
        modelBuilder.Entity<InBoxEvent>(entity =>
        {
            entity.HasKey(e => e.Id)
            .IsClustered(false);
            
            entity.ToTable("_InBoxEvents");
            
            entity.HasIndex(e => e.ReceivedDate, "IX__InBoxEvents_ReceivedDate")
            .IsUnique()
            .IsClustered();
            
            entity.Property(e => e.Id).ValueGeneratedNever();
            
            entity.Property(e => e.MessageId).HasMaxLength(50);
            
            entity.Property(e => e.OwnerService).HasMaxLength(100);
        });
        
        modelBuilder.Entity<OutBoxEvent>(entity =>
        {
            entity.HasKey(e => e.Id)
            .IsClustered(false);
            
            entity.ToTable("_OutBoxEvents");
            
            entity.HasIndex(e => e.AccuredOn, "IX__OutBoxEvents_AccuredOn")
            .IsUnique()
            .IsClustered();
            
            entity.Property(e => e.Id).ValueGeneratedNever();
            
            entity.Property(e => e.AccuredByUserId).HasMaxLength(40);
            
            entity.Property(e => e.AggregateId).HasMaxLength(40);
            
            entity.Property(e => e.AggregateName).HasMaxLength(200);
            
            entity.Property(e => e.AggregateTypeName).HasMaxLength(500);
            
            entity.Property(e => e.EventName).HasMaxLength(100);
            
            entity.Property(e => e.EventTypeName).HasMaxLength(500);
        });
        
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id)
            .IsClustered(false);
            
            entity.HasIndex(e => e.CreatedDate, "IX_Products_CreatedDate")
            .IsUnique()
            .IsClustered();
            
            entity.Property(e => e.Id).ValueGeneratedNever();
            
            entity.Property(e => e.Barcode).HasMaxLength(10);
            
            entity.Property(e => e.Name).HasMaxLength(50);
        });
        
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshToken", "Identity");
            
            entity.HasIndex(e => e.ApplicationUserId, "IX_RefreshToken_ApplicationUserId");
            
            entity.HasOne(d => d.ApplicationUser)
            .WithMany(p => p.RefreshTokens)
            .HasForeignKey(d => d.ApplicationUserId);
        });
        
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role", "Identity");
            
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
            .IsUnique()
            .HasFilter("([NormalizedName] IS NOT NULL)");
            
            entity.Property(e => e.Name).HasMaxLength(256);
            
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });
        
        modelBuilder.Entity<RoleClaim>(entity =>
        {
            entity.ToTable("RoleClaims", "Identity");
            
            entity.HasIndex(e => e.RoleId, "IX_RoleClaims_RoleId");
            
            entity.Property(e => e.RoleId).IsRequired();
            
            entity.HasOne(d => d.Role)
            .WithMany(p => p.RoleClaims)
            .HasForeignKey(d => d.RoleId);
        });
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User", "Identity");
            
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");
            
            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
            .IsUnique()
            .HasFilter("([NormalizedUserName] IS NOT NULL)");
            
            entity.Property(e => e.Email).HasMaxLength(256);
            
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            
            entity.Property(e => e.UserName).HasMaxLength(256);
        });
        
        modelBuilder.Entity<UserClaim>(entity =>
        {
            entity.ToTable("UserClaims", "Identity");
            
            entity.HasIndex(e => e.UserId, "IX_UserClaims_UserId");
            
            entity.Property(e => e.UserId).IsRequired();
            
            entity.HasOne(d => d.User)
            .WithMany(p => p.UserClaims)
            .HasForeignKey(d => d.UserId);
        });
        
        modelBuilder.Entity<UserLogin>(entity =>
        {
        entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            
            entity.ToTable("UserLogins", "Identity");
            
            entity.HasIndex(e => e.UserId, "IX_UserLogins_UserId");
            
            entity.Property(e => e.UserId).IsRequired();
            
            entity.HasOne(d => d.User)
            .WithMany(p => p.UserLogins)
            .HasForeignKey(d => d.UserId);
        });
        
        modelBuilder.Entity<UserRole>(entity =>
        {
        entity.HasKey(e => new { e.UserId, e.RoleId });
            
            entity.ToTable("UserRoles", "Identity");
            
            entity.HasIndex(e => e.RoleId, "IX_UserRoles_RoleId");
            
            entity.HasOne(d => d.Role)
            .WithMany(p => p.UserRoles)
            .HasForeignKey(d => d.RoleId);
            
            entity.HasOne(d => d.User)
            .WithMany(p => p.UserRoles)
            .HasForeignKey(d => d.UserId);
        });
        
        modelBuilder.Entity<UserToken>(entity =>
        {
        entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            
            entity.ToTable("UserTokens", "Identity");
            
            entity.HasOne(d => d.User)
            .WithMany(p => p.UserTokens)
            .HasForeignKey(d => d.UserId);
        });
        
        OnModelCreatingPartial(modelBuilder);
    }
    
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}


