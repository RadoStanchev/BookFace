using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BookFace.Data;
using BookFace.Data.Models;
using System;
using System.Threading.Tasks;
using static BookFace.Areas.Admin.AdminConstants;

namespace BookFace.Infrastructure.Extensions
{

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedAdministrator(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<ApplicationDbContext>();

            data.Database.Migrate();
        }


        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "admin@admin.com";
                    const string adminPassword = "admin2004";

                    var user = new ApplicationUser
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                        FirstName = "Admin",
                        LastName = "Admin",
                        ProfileImagePath = "D:\\OneDrive - Software University (SoftUni)\\C#_projects\\C# Web Projects\\Questions-of-Runeterra-ASP.NET-Core-Project\\QuestionsOfRuneterra\\wwwroot\\images\\admin\\yönetici-png-8.png"
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
