using AiplBlazor.Domain.Common.Entities;

namespace AiplBlazor.Domain.Common.Events;

public class UpdatedEvent<T> : DomainEvent where T : IEntity
{
    public UpdatedEvent(T entity)
    {
        Entity = entity;
    }

    public T Entity { get; }
}