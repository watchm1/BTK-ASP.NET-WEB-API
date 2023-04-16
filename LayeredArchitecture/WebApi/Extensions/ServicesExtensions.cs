using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore;

namespace WebApi.Extensions;

public static class ServicesExtensions
{
    public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RepositoryContext>(options =>
            options.UseMySql(configuration.GetConnectionString("SqlConnection"),
                Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"))); 
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();
}