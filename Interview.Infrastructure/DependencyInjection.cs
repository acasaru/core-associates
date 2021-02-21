using Interview.Application.Interfaces;
using Interview.Infrastructure.Authentication;
using Interview.Infrastructure.DateTimeServices;
using Interview.Infrastructure.Persistence;
using Interview.Infrastructure.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Interview.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer();

            services.AddScoped(_ =>
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)).Options;

                return options;
            });

            services.AddDbContext<ApplicationDbContext>();

            services.AddScoped(typeof(IApplicationRepository<>), typeof(ApplicationRepository<>));
            services.AddScoped(typeof(IApplicationUnitOfWork), typeof(ApplicationUnitOfWork));

            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IAutheticatedUserService, AutheticatedUserService>();
            services.AddTransient<IApplicationSerializationService, ApplicationSerializationService>();

            return services;
        }
    }
}
