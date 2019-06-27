using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace LibraProgramming.ChatRoom.Services.Chat.Persistence
{
    public sealed class PersistedGrantDbContextFactory : DesignTimeDbContextFactoryBase<PersistedGrantDbContext>
    {
        public PersistedGrantDbContextFactory() 
            : base("Identity", typeof(ChatIdentityDbContext).Assembly.GetName().Name)
        {
        }

        protected override PersistedGrantDbContext CreateNewInstance(DbContextOptions<PersistedGrantDbContext> options) =>
            new PersistedGrantDbContext(options, new OperationalStoreOptions());
    }
}