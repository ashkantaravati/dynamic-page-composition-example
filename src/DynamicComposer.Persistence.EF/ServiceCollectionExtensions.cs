using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicComposer.Persistence.EF;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDynamicComposerPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DynamicComposerDbContext>(options => options.UseSqlite(connectionString));
        return services;
    }
}
