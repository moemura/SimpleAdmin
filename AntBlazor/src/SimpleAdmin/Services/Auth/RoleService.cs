using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SimpleAdmin.Services.Auth;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<AuthResponseDto> CreateAsync(RoleDto dto)
    {
        var role = new IdentityRole()
        {
            Name = dto.Name,
            NormalizedName = dto.NormalizedName,
        };
        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
        {
            return new AuthResponseDto { Success = false, Message = "Role not created" };
        }
        else
        {
            return new AuthResponseDto { Success = true, Message = "Role created" };
        }
    }
    
    public async Task DeleteAsync(string id) 
    {
        var role = await _roleManager.FindByIdAsync(id) ?? new IdentityRole();
        var result = await _roleManager.DeleteAsync(role);
    }

    public async Task UpdateAsync(RoleDto dto)
    {
        var role = await _roleManager.FindByIdAsync(dto.Id) ?? new IdentityRole();
        role.Name = dto.Name;
        role.NormalizedName = dto.NormalizedName;
        var result = await _roleManager.UpdateAsync(role);
    }

    public async Task AssignRoleAsync(RoleDto dto, IEnumerable<UserDto> users)
    {
        foreach (var userDto in users)
        {
            var user =  await _userManager.FindByIdAsync(userDto.Id) ?? new ApplicationUser();
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(dto.Name))
            {
                await _userManager.AddToRoleAsync(user, dto.Name);
            }
        }
    }

    public async Task RemoveRoleAsync(RoleDto dto, IEnumerable<UserDto> users)
    {
        foreach (var userDto in users)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id) ?? new ApplicationUser();
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(dto.Name))
            {
                await _userManager.RemoveFromRoleAsync(user, dto.Name);
            }
        }
    }

    public async Task<IEnumerable<UserDto>> GetUsersInRoleAsync(string roleName)
    {
        var users = await _userManager.GetUsersInRoleAsync(roleName);
        return users.Select(x => new UserDto() { Id = x.Id, Email = x.Email, FullName = x.FullName });
    }

    public async Task<PaginatedList<RoleDto>> FilterAndPagingRolesAsync(int pageIndex, int pageSize, Dictionary<string, string> filter)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var query = dbContext.Roles.AsQueryable();
        if (filter.TryGetValue("name", out var name) && !string.IsNullOrWhiteSpace(name))
            query = query.Where(u => u.Name.Contains(name));
        
        var totalItems = await query.CountAsync();
        var roles = await query.OrderBy(u => u.Name)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        // Map users to DTOs outside of using block
        var roleDtos = new List<RoleDto>();
        foreach (var role in roles)
        {
            roleDtos.Add(new RoleDto
            {
                Id = role.Id,
                Name = role.Name!,
                NormalizedName = role.NormalizedName ?? string.Empty,
            });
        }
        return new PaginatedList<RoleDto>(roleDtos, pageIndex, pageSize, totalItems);
    }

    private async Task<RoleDto> MapToRoleDtoAsync(IdentityRole role)
    {
        var dto =  new RoleDto
        {
            Id = role.Id,
            Name = role.Name!,
            NormalizedName = role.NormalizedName!,
        };
        return dto;
    }

    public async Task<PaginatedList<RoleDto>> GetPaginationAsync(int pageIndex, int pageSize)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var totalItems = await dbContext.Roles.CountAsync();
        var roles = await dbContext.Roles
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Map roles to RoleDto asynchronously
        var roleDtos = new List<RoleDto>();
        foreach (var role in roles)
        {
            roleDtos.Add(await MapToRoleDtoAsync(role));
        }

        return new PaginatedList<RoleDto>(
            roleDtos,
            pageIndex,
            pageSize,
            totalItems
        );
    }
}