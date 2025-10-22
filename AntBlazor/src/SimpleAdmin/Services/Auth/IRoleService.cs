using SimpleAdmin.Models.DTOs;

namespace SimpleAdmin.Services.Auth;

public interface IRoleService
{
    Task<AuthResponseDto> CreateAsync(RoleDto dto);
    Task DeleteAsync(string id);
    Task UpdateAsync(RoleDto id);
    Task AssignRoleAsync(RoleDto dto, IEnumerable<UserDto> users);
    Task RemoveRoleAsync(RoleDto dto, IEnumerable<UserDto> users);
    Task<IEnumerable<UserDto>> GetUsersInRoleAsync(string roleName);
    Task<PaginatedList<RoleDto>> FilterAndPagingRolesAsync(int pageIndex, int pageSize, Dictionary<string, string> filter);
    Task<PaginatedList<RoleDto>> GetPaginationAsync(int pageIndex, int pageSize);
} 