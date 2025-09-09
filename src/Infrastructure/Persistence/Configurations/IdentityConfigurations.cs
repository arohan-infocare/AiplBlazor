// File: IdentityConfigurations.cs
// Namespace: AiplBlazor.Infrastructure.Persistence.Configurations
// Drop this file into your Infrastructure project (where other IEntityTypeConfiguration classes live).

using System;
using AiplBlazor.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiplBlazor.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Identity default lengths
            builder.Property(u => u.UserName).HasMaxLength(256);
            builder.Property(u => u.NormalizedUserName).HasMaxLength(256);
            builder.Property(u => u.Email).HasMaxLength(256);
            builder.Property(u => u.NormalizedEmail).HasMaxLength(256);

            // Relationships (explicit)
            builder.HasMany(u => u.Logins)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Tokens)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserClaims)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Superior and Tenant navigations
            builder.HasOne(u => u.Superior)
                .WithMany()
                .HasForeignKey(u => u.SuperiorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Tenant)
                .WithMany()
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Eager-load tenant sometimes useful — keep with caution
            builder.Navigation(u => u.Tenant).AutoInclude();

            // Concurrency token
            builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
        }
    }

    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            // Identity defaults for lengths
            builder.Property(r => r.Name).HasMaxLength(256);
            builder.Property(r => r.NormalizedName).HasMaxLength(256);

            // Unique constraint: role name within tenant
            builder.HasIndex(r => new { r.TenantId, r.NormalizedName })
                   .IsUnique()
                   .HasDatabaseName("IX_Roles_TenantId_NormalizedName");

            // Keep a non-unique index on NormalizedName to support queries that look up roles
            builder.HasIndex(r => r.NormalizedName)
                   .HasDatabaseName("IX_Roles_NormalizedName");

            builder.HasOne(r => r.Tenant)
                   .WithMany()
                   .HasForeignKey(r => r.TenantId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Auto include Tenant if desired (be mindful of performance)
            builder.Navigation(r => r.Tenant).AutoInclude();

            // Concurrency token
            builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
        }
    }

    public class ApplicationRoleClaimConfiguration : IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
        {
            builder.HasOne(rc => rc.Role)
                   .WithMany(r => r.RoleClaims)
                   .HasForeignKey(rc => rc.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Optional: if you added TenantId to role claims (scoped claims), ensure index
            // builder.Property(rc => rc.TenantId).HasMaxLength(64);
            // builder.HasIndex(rc => new { rc.RoleId, rc.TenantId }).HasDatabaseName("IX_RoleClaims_RoleId_TenantId");
        }
    }

    public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.HasOne(ur => ur.Role)
                   .WithMany(r => r.UserRoles)
                   .HasForeignKey(ur => ur.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.User)
                   .WithMany(u => u.UserRoles)
                   .HasForeignKey(ur => ur.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Composite key might already exist in Identity schema (userId, roleId)
            builder.HasIndex(ur => new { ur.UserId, ur.RoleId })
                   .HasDatabaseName("IX_UserRoles_UserId_RoleId");
        }
    }

    public class ApplicationUserClaimConfiguration : IEntityTypeConfiguration<ApplicationUserClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
        {
            builder.HasOne(uc => uc.User)
                   .WithMany(u => u.UserClaims)
                   .HasForeignKey(uc => uc.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Index to speed up lookup by claim type/value if needed
            builder.HasIndex(uc => new { uc.ClaimType, uc.ClaimValue })
                   .HasDatabaseName("IX_UserClaims_Type_Value");
        }
    }

    public class ApplicationUserLoginConfiguration : IEntityTypeConfiguration<ApplicationUserLogin>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder)
        {
            builder.HasOne(l => l.User)
                   .WithMany(u => u.Logins)
                   .HasForeignKey(l => l.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Composite PK is typically (LoginProvider, ProviderKey) in ASP.NET Identity
            builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });
        }
    }

    public class ApplicationUserTokenConfiguration : IEntityTypeConfiguration<ApplicationUserToken>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
        {
            builder.HasOne(t => t.User)
                   .WithMany(u => u.Tokens)
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Composite PK is typically (UserId, LoginProvider, Name)
            builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
        }
    }
}
