using AiplBlazor.Application.Features.Tenants.DTOs;

namespace AiplBlazor.Application.Common.Interfaces.MultiTenant;

public interface ITenantService
{
    List<TenantDto> DataSource { get; }
    event Func<Task>? OnChange;
    Task InitializeAsync();
    Task RefreshAsync();
}