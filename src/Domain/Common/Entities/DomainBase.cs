// File: DomainBase.cs
// Put in AiplBlazor.Domain.Common.Entities (or existing domain common folder)

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AiplBlazor.Domain.Common; // DomainEvent lives here (your existing file)

namespace AiplBlazor.Domain.Common.Entities;

public interface IAuditableEntity
{
    DateTime? CreatedAt { get; set; }

    string? CreatedById { get; set; }

    DateTime? LastModifiedAt { get; set; }

    string? LastModifiedById { get; set; }
}
// -------------------------------------------------------------------------
// Generic base entity with domain event support
// -------------------------------------------------------------------------
public abstract class BaseEntity<TKey> : IEntity<TKey>
    where TKey : notnull
{
    // Domain events
    [NotMapped]
    private readonly List<DomainEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Primary key (specialize for int/guid via concrete subclasses below).
    /// </summary>
    public virtual TKey Id { get; set; } = default!;

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        if (domainEvent is null) throw new ArgumentNullException(nameof(domainEvent));
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        if (domainEvent is null) return;
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}

// -------------------------------------------------------------------------
// Compatibility specializations (int and guid)
// -------------------------------------------------------------------------

/// <summary>
/// Compatibility: matches the original template's BaseEntity : IEntity<int>.
/// Use for entities that intentionally want INT PKs (small lookup/reference tables).
/// </summary>
public abstract class BaseEntity : BaseEntity<int>
{
    // Int identity semantics: leave Id = 0 until DB assigns identity on insert.
}

/// <summary>
/// GUID specialization if/when you choose to use GUID PKs in some entities.
/// (Not strictly required to compile template; provided for completeness.)
/// </summary>
public abstract class BaseEntityGuid : BaseEntity<Guid>
{
    protected BaseEntityGuid()
    {
        if (EqualityComparer<Guid>.Default.Equals(Id, default))
        {
            Id = Guid.NewGuid();
        }
    }
}

// -------------------------------------------------------------------------
// Auditable base classes (int PK) matching template naming
// -------------------------------------------------------------------------

/// <summary>
/// Base auditable entity (int PK) - matches your existing PicklistSet usage.
/// </summary>
public abstract class BaseAuditableEntity : BaseEntity
{
    public virtual DateTime? CreatedAt { get; set; }
    public virtual string? CreatedById { get; set; }
    public virtual DateTime? LastModifiedAt { get; set; }
    public virtual string? LastModifiedById { get; set; }
}

/// <summary>
/// Base auditable + soft-delete entity (int PK).
/// </summary>
public abstract class BaseAuditableSoftDeleteEntity : BaseAuditableEntity, ISoftDelete
{
    public virtual bool IsDeleted { get; set; }
    public virtual DateTime? DeletedAt { get; set; }
    public virtual string? DeletedById { get; set; }
}

// -------------------------------------------------------------------------
// If you want GUID auditable variants, add them too (optional)
// -------------------------------------------------------------------------

public abstract class BaseAuditableEntityGuid : BaseEntityGuid
{
    public virtual DateTime? CreatedAt { get; set; }
    public virtual string? CreatedById { get; set; }
    public virtual DateTime? LastModifiedAt { get; set; }
    public virtual string? LastModifiedById { get; set; }
}

public abstract class BaseAuditableSoftDeleteEntityGuid : BaseAuditableEntityGuid, ISoftDelete
{
    public virtual bool IsDeleted { get; set; }
    public virtual DateTime? DeletedAt { get; set; }
    public virtual string? DeletedById { get; set; }
}
