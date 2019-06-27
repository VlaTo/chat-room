using Microsoft.EntityFrameworkCore;

namespace LibraProgramming.ChatRoom.Services.Chat.Persistence
{
    public sealed class ChatIdentityDbContextFactory : DesignTimeDbContextFactoryBase<ChatIdentityDbContext>
    {
        public ChatIdentityDbContextFactory()
            : base("Chat", typeof(ChatIdentityDbContext).Assembly.GetName().Name)
        {
        }

        protected override ChatIdentityDbContext CreateNewInstance(DbContextOptions<ChatIdentityDbContext> options) =>
            new ChatIdentityDbContext(options);
    }
}