using Microsoft.AspNetCore.Identity;
using SimpleAdmin.Models.DTOs;

namespace SimpleAdmin.Services.Auth;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto model);
    Task<AuthResponseDto> LoginAsync(LoginDto model);
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    Task<bool> RevokeTokenAsync(string refreshToken);
    Task<AuthResponseDto> ChangePasswordAsync(string userId, ChangePasswordDto model);
    Task<AuthResponseDto> UpdateProfileAsync(string userId, UpdateProfileDto model);
    Task<AuthResponseDto> GetProfileAsync(string userId);
    Task<bool> DeleteAccountAsync(string userId, string password);
    Task<bool> ForgotPasswordAsync(ForgotPasswordDto model);
    Task<bool> ResetPasswordAsync(ResetPasswordDto model);
    Task<bool> LockAccountAsync(string userId, DateTime? lockoutEnd);
    Task<bool> UnlockAccountAsync(string userId);
    Task<bool> DeactivateAccountAsync(string userId);
    Task<bool> ReactivateAccountAsync(string userId);
} 