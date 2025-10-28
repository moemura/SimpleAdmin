using System.ComponentModel.DataAnnotations;

namespace SimpleAdmin.Models.DTOs;

public class AdminLoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [Required]
    public bool RememberMe { get; set; } = true;
} 