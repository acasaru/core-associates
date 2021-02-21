using Interview.Application.Core.Entitities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Interview.Infrastructure.Persistence
{
    public static class PersistenceExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>().HasData(
                new UserInfo { ApiKey = "admin123", UserId = 1, Role = "Admin" },
                new UserInfo { ApiKey = "admin345", UserId = 2, Role = "Admin" },
                new UserInfo { ApiKey = "user123", UserId = 3, Role = "User" },
                new UserInfo { ApiKey = "user345", UserId = 4, Role = "User" }
            );
        }

        public static IHost MigrateDatabase(this IHost host)
        {
            using( var scope  =  host.Services.CreateScope())
            {
                using(var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    appContext.Database.Migrate();
                }
            }

            return host;
        }
    }
}
