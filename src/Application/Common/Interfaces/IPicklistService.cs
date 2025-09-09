using AiplBlazor.Application.Features.PicklistSets.DTOs;

namespace AiplBlazor.Application.Common.Interfaces;

public interface IPicklistService
{
    List<PicklistSetDto> DataSource { get; }
    event Func<Task>? OnChange;
    Task InitializeAsync();
    Task RefreshAsync();
}