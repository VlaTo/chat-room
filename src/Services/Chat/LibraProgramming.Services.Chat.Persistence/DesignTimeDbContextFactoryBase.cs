using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LibraProgramming.ChatRoom.Services.Chat.Persistence
{
    public abstract class DesignTimeDbContextFactoryBase<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
    where TDbContext: DbContext
    {
        private const string EnvironmentVariableName = "ASPNETCORE_ENVIRONMENT";

        protected string ConnectionStringName
        {
            get;
        }

        protected string MigrationsAssemblyName
        {
            get;
        }

        protected DesignTimeDbContextFactoryBase(string connectionStringName, string migrationsAssemblyName)
        {
            ConnectionStringName = connectionStringName;
            MigrationsAssemblyName = migrationsAssemblyName;
        }

        public TDbContext CreateDbContext(string[] args)
        {
            return Create(
                Path.Combine(Directory.GetCurrentDirectory(), "..\\LibraProgramming.Services.Chat.Api"),
                //Directory.GetCurrentDirectory(),
                Environment.GetEnvironmentVariable(EnvironmentVariableName),
                ConnectionStringName,
                MigrationsAssemblyName
            );
        }

        public TDbContext CreateWithConnectionStringName(string connectionStringName, string migrationsAssemblyName)
        {
            var environmentName = Environment.GetEnvironmentVariable(EnvironmentVariableName);
            var basePath = AppContext.BaseDirectory;

            return Create(basePath, environmentName, connectionStringName, migrationsAssemblyName);
        }

        protected abstract TDbContext CreateNewInstance(DbContextOptions<TDbContext> options);

        private TDbContext Create(string basePath, string environmentName, string connectionStringName, string migrationsAssemblyName)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString(connectionStringName);

            Console.WriteLine($"Using connection string \'{connectionString}\'");

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException($"Could not find a connection string named \'{connectionString}\'.");
            }

            return CreateWithConnectionString(connectionString, migrationsAssemblyName);
        }

        private TDbContext CreateWithConnectionString(string connectionString, string migrationsAssemblyName)
        {
            if (String.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"{nameof(connectionString)} is null or empty.", nameof(connectionString));
            }

            var context = new DbContextOptionsBuilder<TDbContext>();

            context.UseSqlite(connectionString, options =>
            {
                options.MigrationsAssembly(migrationsAssemblyName);
            });

            return CreateNewInstance(context.Options);
        }
    }
}