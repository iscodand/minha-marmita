using Application.Contracts.Services;
using Domain.Entities;
using Infrastructure.Data.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Extensions
{
    public static class AppExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
            serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
        }

        public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            {
                UserManager<User> userManager = (UserManager<User>)scope
                    .ServiceProvider
                    .GetService(typeof(UserManager<User>));

                ICompanyService companyService = (ICompanyService)scope
                    .ServiceProvider
                    .GetService(typeof(ICompanyService));

                RoleManager<Role> roleManager = (RoleManager<Role>)scope
                    .ServiceProvider
                    .GetService(typeof(RoleManager<Role>));

                await Infrastructure.Identity.Seeds.DefaultRoles.SeedAsync(roleManager);
                await Infrastructure.Identity.Seeds.DefaultCompany.SeedAsync(companyService);
                await Infrastructure.Identity.Seeds.DefaultAdmin.SeedAsync(userManager, companyService);
            }
        }
    }
}