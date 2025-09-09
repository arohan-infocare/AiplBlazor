using AiplBlazor.Application.Features.Identity.DTOs;

namespace AiplBlazor.Application.Common.Interfaces.Identity;

public interface IRoleService
{
    List<ApplicationRoleDto> DataSource { get; }
    event Func<Task>? OnChange;
    Task InitializeAsync();
    Task  RefreshAsync();
}