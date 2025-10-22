using SimpleAdmin.Models.DTOs;

namespace SimpleAdmin.Services.Auth;

public interface IAccountService
{
    Task<AuthResponseDto> CreateAsync(RegisterDto dto);
    Task DeleteAsync(string id);
    Task UpdateAsync(UserDto id);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<AuthResponseDto> ChangePasswordAsync(string userId, ChangePasswordDto model);
    Task<AuthResponseDto> UpdateProfileAsync(string userId, UpdateProfileDto model);
    Task<AuthResponseDto> GetProfileAsync(string userId);
    Task<bool> DeleteAccountAsync(string userId, string password);
    Task<bool> LockAccountAsync(string userId, DateTime? lockoutEnd);
    Task<bool> UnlockAccountAsync(string userId);
    Task<bool> DeactivateAccountAsync(string userId);
    Task<bool> ReactivateAccountAsync(string userId);
    Task<PaginatedList<UserDto>> FilterAndPagingUsersAsync(int pageIndex, int pageSize, Dictionary<string, string> filter);
    Task<PaginatedList<UserDto>> GetPaginationAsync(int pageIndex, int pageSize);
} 