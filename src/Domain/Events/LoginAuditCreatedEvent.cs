using AiplBlazor.Domain.Identity;

namespace AiplBlazor.Domain.Events;

    public class LoginAuditCreatedEvent : DomainEvent
    {
        public LoginAuditCreatedEvent(LoginAudit item)
        {
            Item = item;
        }

        public LoginAudit Item { get; }
    }

