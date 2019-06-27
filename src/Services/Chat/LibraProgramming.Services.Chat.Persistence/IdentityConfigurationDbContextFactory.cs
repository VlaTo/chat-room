using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace LibraProgramming.ChatRoom.Services.Chat.Persistence
{
    public sealed class IdentityConfigurationDbContextFactory : DesignTimeDbContextFactoryBase<ConfigurationDbContext>
    {
        public IdentityConfigurationDbContextFactory()
            : base("Identity", typeof(ChatIdentityDbContext).Assembly.GetName().Name)
        {
        }

        protected override ConfigurationDbContext CreateNewInstance(DbContextOptions<ConfigurationDbContext> options) =>
            new ConfigurationDbContext(options, new ConfigurationStoreOptions());
    }
}