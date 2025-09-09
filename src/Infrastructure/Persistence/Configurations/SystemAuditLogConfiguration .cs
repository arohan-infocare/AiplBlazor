using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiplBlazor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiplBlazor.Infrastructure.Persistence.Configurations;
public class SystemAuditLogConfiguration : IEntityTypeConfiguration<SystemAuditLog>
{
    public void Configure(EntityTypeBuilder<SystemAuditLog> builder)
    {
        builder.ToTable("SystemAuditLogs"); // clear table name

        // Columns
        builder.Property(x => x.UserId).HasMaxLength(128).IsUnicode(false);
        builder.Property(x => x.ActionType).HasMaxLength(64).IsUnicode(false);
        builder.Property(x => x.EntityAffected).HasMaxLength(128).IsUnicode(false);
        builder.Property(x => x.EntityId).HasMaxLength(64).IsUnicode(false);
        builder.Property(x => x.TimestampUtc).IsRequired();
        builder.Property(x => x.Details).IsUnicode(true); // allow unicode for JSON/text
        builder.Property(x => x.CorrelationId).HasMaxLength(64).IsUnicode(false);
        builder.Property(x => x.TenantId).HasMaxLength(64).IsUnicode(false);

        // Indexes — tuned for common queries and retention filters
        builder.HasIndex(x => x.UserId)
               .HasDatabaseName("IX_SystemAuditLogs_UserId");

        builder.HasIndex(x => x.ActionType)
               .HasDatabaseName("IX_SystemAuditLogs_ActionType");

        builder.HasIndex(x => x.EntityAffected)
               .HasDatabaseName("IX_SystemAuditLogs_EntityAffected");

        builder.HasIndex(x => x.TimestampUtc)
               .HasDatabaseName("IX_SystemAuditLogs_TimestampUtc");

        builder.HasIndex(x => x.CorrelationId)
               .HasDatabaseName("IX_SystemAuditLogs_CorrelationId");

        // Composite index for user timeline lookups: user -> time desc
        builder.HasIndex(x => new { x.UserId, x.TimestampUtc })
               .HasDatabaseName("IX_SystemAuditLogs_UserId_TimestampUtc");

        // Optional: partitioning hint or annotation could be added here for time-series cleanup tools

        // Query performance tip: if you anticipate frequent queries by Tenant + Timestamp, add:
        builder.HasIndex(x => new { x.TenantId, x.TimestampUtc })
               .HasDatabaseName("IX_SystemAuditLogs_TenantId_TimestampUtc");
    }
}
