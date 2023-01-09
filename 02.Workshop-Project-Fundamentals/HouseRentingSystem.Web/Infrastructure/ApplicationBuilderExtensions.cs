using HouseRentingSystem.Services.Data.Entities;
using HouseRentingSystem.Web.Areas.Admin;
using Microsoft.AspNetCore.Identity;

namespace HouseRentingSystem.Web.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedAdmin(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var services = scopedServices.ServiceProvider;

            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(AdminConstants.AdminRoleName))
                {
                    return;
                }

                var role = new IdentityRole { Name = AdminConstants.AdminRoleName }; ;
                await roleManager.CreateAsync(role);

                var admin = await userManager.FindByNameAsync(AdminConstants.AdminEmail);
                await userManager.AddToRoleAsync(admin, role.Name);
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }
    }
}
