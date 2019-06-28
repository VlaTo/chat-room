using System;
using System.Data.Common;
using System.IO;
using LibraProgramming.ChatRoom.Client.Models.Database;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Persistence
{
    public class ChatDatabaseContext : DbContext
    {
        private const string filename = "chat.db";

        public virtual DbSet<Message> Messages
        {
            get;
            set;
        }

        public ChatDatabaseContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var databaseName = String.Empty;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                {
                    SQLitePCL.Batteries_V2.Init();

                    databaseName = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        "..",
                        "Library",
                        filename
                    );

                    break;
                }

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

                default:
                {
                    throw new NotImplementedException("Platform not supported");
                }
            }

            var builder = new DbConnectionStringBuilder
            {
                {"Filename", databaseName}
            };

            optionsBuilder.UseSqlite(builder.ConnectionString);
        }
    }
}