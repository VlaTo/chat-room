using LibraProgramming.ChatRoom.Services.Chat.Persistence.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraProgramming.ChatRoom.Services.Chat.Persistence
{
    public class ChatIdentityDbContext : IdentityDbContext<Customer>
    {
        public ChatIdentityDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
