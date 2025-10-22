using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleAdmin.Data;
using SimpleAdmin.Models.Entities;

namespace SimpleAdmin.Services.Auth;

public class DbSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DbSeeder> _logger;
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public DbSeeder(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        ILogger<DbSeeder> logger,
        IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
        _dbContextFactory = dbContextFactory;
    }

    public async Task SeedAsync()
    {
        try
        {
            // Create roles if they don't exist
            await CreateRolesAsync();

            // Create admin user if it doesn't exist
            await CreateAdminUserAsync();

            _logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private async Task CreateRolesAsync()
    {
        var roles = new[] { "Admin", "Customer", "Staff" };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
                _logger.LogInformation($"Created role: {role}");
            }
        }
    }

    private async Task CreateAdminUserAsync()
    {
        var adminEmail = _configuration["AdminUser:Email"] ?? "admin@simpleadmin.com";
        var adminPassword = _configuration["AdminUser:Password"] ?? "Admin@123";

        var adminUser = await _userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                PhoneNumber = "0123456789",
                FullName = "System Administrator",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                _logger.LogInformation("Created admin user successfully");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError($"Failed to create admin user: {errors}");
            }
        }
        else
        {
            // Ensure admin user has Admin role
            if (!await _userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                _logger.LogInformation("Added Admin role to existing admin user");
            }
        }
    }
} 