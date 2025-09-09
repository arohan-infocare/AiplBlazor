namespace AiplBlazor.Domain.Identity;


public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    public string? Description { get; set; }
    public string? Group { get; set; }
    public string? TenantId { get; set; }    // <--- add if you want claim-scoped tenancy
    public virtual ApplicationRole Role { get; set; } = default!;
}