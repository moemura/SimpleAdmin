using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SimpleAdmin.Services.Auth;

namespace SimpleAdmin.Controllers;

/// <summary>
/// Controller quản lý tài khoản người dùng
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// Đổi mật khẩu tài khoản hiện tại
    /// </summary>
    /// <param name="model">Thông tin đổi mật khẩu (mật khẩu cũ và mật khẩu mới)</param>
    /// <returns>Kết quả đổi mật khẩu</returns>
    /// <response code="200">Đổi mật khẩu thành công</response>
    /// <response code="400">Thông tin không hợp lệ hoặc mật khẩu cũ sai</response>
    /// <response code="401">Chưa đăng nhập</response>
    [HttpPost("change-password")]
    [ProducesResponseType(typeof(AuthResponseDto), 200)]
    [ProducesResponseType(typeof(AuthResponseDto), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<AuthResponseDto>> ChangePassword(ChangePasswordDto model)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        var result = await _accountService.ChangePasswordAsync(userId, model);
        if (!result.Success)
            return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Cập nhật thông tin cá nhân
    /// </summary>
    /// <param name="model">Thông tin cá nhân cần cập nhật</param>
    /// <returns>Thông tin tài khoản sau khi cập nhật</returns>
    /// <response code="200">Cập nhật thông tin thành công</response>
    /// <response code="400">Thông tin không hợp lệ</response>
    /// <response code="401">Chưa đăng nhập</response>
    [HttpPut("profile")]
    [ProducesResponseType(typeof(AuthResponseDto), 200)]
    [ProducesResponseType(typeof(AuthResponseDto), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<AuthResponseDto>> UpdateProfile(UpdateProfileDto model)
    {
        bool authenticated = User.Identity.IsAuthenticated;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        var result = await _accountService.UpdateProfileAsync(userId, model);
        if (!result.Success)
            return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Lấy thông tin cá nhân của người dùng hiện tại
    /// </summary>
    /// <returns>Thông tin cá nhân của người dùng</returns>
    /// <response code="200">Lấy thông tin thành công</response>
    /// <response code="400">Lỗi khi lấy thông tin</response>
    /// <response code="401">Chưa đăng nhập</response>
    [HttpGet("profile")]
    [ProducesResponseType(typeof(AuthResponseDto), 200)]
    [ProducesResponseType(typeof(AuthResponseDto), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<AuthResponseDto>> GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        var result = await _accountService.GetProfileAsync(userId);
        if (!result.Success)
            return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Xóa tài khoản người dùng (yêu cầu xác nhận mật khẩu)
    /// </summary>
    /// <param name="password">Mật khẩu để xác nhận xóa tài khoản</param>
    /// <returns>Kết quả xóa tài khoản</returns>
    /// <response code="200">Xóa tài khoản thành công</response>
    /// <response code="400">Mật khẩu sai hoặc không thể xóa tài khoản</response>
    /// <response code="401">Chưa đăng nhập</response>
    [HttpDelete("account")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult> DeleteAccount([FromBody] string password)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        var result = await _accountService.DeleteAccountAsync(userId, password);
        if (!result)
            return BadRequest("Sai mật khẩu hoặc không thể xoá tài khoản");
        return Ok(new { Message = "Xoá tài khoản thành công" });
    }
} 