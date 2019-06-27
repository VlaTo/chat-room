using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Extensions
{
    internal static class WebHostExtensions
    {
        public static IWebHost MigrateDbContext<TDbContext>(this IWebHost host, Func<TDbContext, IServiceProvider, Task> seeder)
            where TDbContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<TDbContext>();

                try
                {
                    context.Database.Migrate();
                    seeder.Invoke(context, scope.ServiceProvider).Wait();
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<TDbContext>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");

                }
            }

            return host;
        }
    }
}