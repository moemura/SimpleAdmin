using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SimpleAdmin.Data;

public class SimpleAdminDbContext : IdentityDbContext<ApplicationUser>
{
    public SimpleAdminDbContext(DbContextOptions<SimpleAdminDbContext> options) : base(options) { }
    public SimpleAdminDbContext() { }

    public virtual DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}