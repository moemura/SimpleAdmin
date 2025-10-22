using SimpleAdmin.Services.Auth;
using SimpleAdmin.Services.Catches;
using SimpleAdmin.Services.Categories;
using SimpleAdmin.Services.Files;

namespace SimpleAdmin;

public static class ServiceContainer
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<ICacheService, NoCacheService>();
        //services.AddScoped<ICacheService, MemoryCacheService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IImageStorageService, ImgurService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IRoleService, RoleService>();

        services.AddHttpClient();
    }
    public static void AddExternalServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddAntDesign();
        services.AddScoped<IEmailSender, EmailSender>();
    }
}
