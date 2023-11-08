using BackendService.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendService
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(builderOptions =>
            {
                builderOptions.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), options =>
                {
                    options.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(60), errorNumbersToAdd: null);
                });
            });
            return services;
        }
        public static IServiceCollection AddRequiredService(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            //services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
