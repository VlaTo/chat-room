using System;
using System.Data.Common;
using System.IO;
using LibraProgramming.ChatRoom.Client.Models.Data;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Persistence
{
    public class ChatDbContext : DbContext
    {
        private const string filename = "chat.db";

        public virtual DbSet<Message> Messages
        {
            get;
            set;
        }

        public virtual DbSet<Room> Rooms
        {
            get;
            set;
        }

        public ChatDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var databaseName = String.Empty;

            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                {
                    databaseName = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                        filename
                    );

                        break;
                }

                case Device.UWP:
                {
                    databaseName = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        filename
                    );

                    break;
                }
            }

            var connectionBuilder = new DbConnectionStringBuilder
            {
                {"Filename", databaseName}
            };

            optionsBuilder.UseSqlite(connectionBuilder.ConnectionString);
        }
    }
}