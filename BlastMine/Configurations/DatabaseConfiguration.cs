using BlastMine.Data;
using BlastMine.Shared;

namespace BlastMine.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        DatabaseSettings? databaseSettings
    )
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(databaseSettings?.ConnectionString));

        return services;
    }
}
