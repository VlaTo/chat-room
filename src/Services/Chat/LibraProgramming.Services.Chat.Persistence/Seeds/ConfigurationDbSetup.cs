using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using LibraProgramming.ChatRoom.Common.Core;
using LibraProgramming.ChatRoom.Services.Chat.Persistence.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LibraProgramming.ChatRoom.Services.Chat.Persistence.Seeds
{
    public class ConfigurationDbSetup
    {
        private readonly IHostingEnvironment environment;
        private readonly UserManager<Customer> customerManager;
        private readonly RoleManager<CustomerRole> roleManager;
        private readonly ConfigurationDbContext configurationDbContext;
        private readonly ILogger<ConfigurationDbSetup> logger;

        public ConfigurationDbSetup(
            IHostingEnvironment environment,
            UserManager<Customer> customerManager,
            RoleManager<CustomerRole> roleManager,
            ConfigurationDbContext configurationDbContext,
            ILoggerFactory logger)
        {
            this.environment = environment;
            this.customerManager = customerManager;
            this.roleManager = roleManager;
            this.configurationDbContext = configurationDbContext;
            this.logger = logger.CreateLogger<ConfigurationDbSetup>();
        }

        public async Task SeedAsync(
            IEnumerable<Client> configuredClients,
            IEnumerable<IdentityResource> configuredIdentityResources,
            IEnumerable<ApiResource> configuredApiResources)
        {
            await SeedRolesAsync();
            await SeedCustomersAsync();

            using (var scope = await configurationDbContext.Database.BeginTransactionAsync())
            {
                await SeedIdentityServerAsync(configuredClients, configuredIdentityResources, configuredApiResources);

                scope.Commit();
            }
        }

        private async Task SeedRolesAsync()
        {
            if (customerManager.SupportsUserRole)
            {
                var roles = new[]
                {
                    StandardRoles.Administrator,
                    StandardRoles.Customer
                };

                foreach (var roleName in roles)
                {
                    var role = await roleManager.FindByNameAsync(roleName);

                    if (null != role)
                    {
                        continue;
                    }

                    var result = await roleManager.CreateAsync(new CustomerRole
                    {
                        Name = roleName
                    });

                    if (false == result.Succeeded)
                    {
                        var descriptions = result.Errors.Select(error => error.Description);
                        throw new Exception(String.Join(';', descriptions));
                    }
                }

            }
        }

        private async Task SeedCustomersAsync()
        {
            var users = new[]
            {
                (
                    Email: "admin@storyblog.net",
                    UserName: "admin",
                    Password: "Abcd1234!",
                    ContactName: "Administrator",
                    Roles: new[]
                    {
                        StandardRoles.Administrator,
                        StandardRoles.Customer
                    }
                ),
                (
                    Email: "dev@storyblog.net",
                    UserName: "dev",
                    Password: "1234Abcd!",
                    ContactName: "Developer",
                    Roles: new[]
                    {
                        StandardRoles.Customer
                    }
                ),
                (
                    Email: "test@storyblog.net",
                    UserName: "test",
                    Password: "Test1234!",
                    ContactName: "Test user",
                    Roles: new[]
                    {
                        StandardRoles.Customer
                    }
                )
            };

            foreach (var user in users)
            {
                Customer customer = null;

                for (var count = 1; count >= 0; count--)
                {
                    customer = await customerManager.FindByEmailAsync(user.Email);

                    if (null != customer)
                    {
                        break;
                    }

                    var result = await customerManager.CreateAsync(new Customer
                    {
                        UserName = user.UserName,
                        Email = user.Email
                    });

                    if (false == result.Succeeded)
                    {
                        break;
                    }
                }

                if (null == customer)
                {
                    throw new Exception("Failed to create customer");
                }

                if (customerManager.SupportsUserPassword && false == String.IsNullOrEmpty(user.Password))
                {
                    if (false == await customerManager.HasPasswordAsync(customer))
                    {
                        var result = await customerManager.AddPasswordAsync(customer, user.Password);

                        if (false == result.Succeeded)
                        {
                            throw new Exception("Failed to add password");
                        }
                    }
                }

                if (customerManager.SupportsUserRole)
                {
                    var roles = await customerManager.GetRolesAsync(customer);

                    foreach (var role in user.Roles)
                    {
                        if (roles.Contains(role))
                        {
                            continue;
                        }

                        var result = await customerManager.AddToRoleAsync(customer, role);

                        if (false == result.Succeeded)
                        {
                            throw new Exception("Failed to role");
                        }
                    }
                }
            }
        }

        private async Task SeedIdentityServerAsync(
            IEnumerable<Client> configuredClients,
            IEnumerable<IdentityResource> configuredIdentityResources,
            IEnumerable<ApiResource> configuredApiResources)
        {
            var clients = configurationDbContext.Clients;

            foreach (var configuredClient in configuredClients)
            {
                if (clients.Any(existing => existing.ClientName == configuredClient.ClientName))
                {
                    continue;
                }

                await clients.AddAsync(configuredClient.ToEntity());
            }

            var identityResources = configurationDbContext.IdentityResources;

            foreach (var resource in configuredIdentityResources)
            {
                if (identityResources.Any(existing => existing.Name == resource.Name))
                {
                    continue;
                }

                identityResources.Add(resource.ToEntity());
            }

            var apiResources = configurationDbContext.ApiResources;

            foreach (var resource in configuredApiResources)
            {
                if (apiResources.Any(existing => existing.Name == resource.Name))
                {
                    continue;
                }

                apiResources.Add(resource.ToEntity());
            }

            configurationDbContext.SaveChanges();
        }
    }
}