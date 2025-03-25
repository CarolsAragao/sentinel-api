using Microsoft.AspNetCore.Identity;
using sentinel_api.Data;
using sentinel_api.Models;

namespace sentinel_api.Services
{
    public class SeedService
    {
        public static async Task SeedDataBase(IServiceProvider service)
        {
            using var scope = service.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            //var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedService>>();

            try
            {
                //logger.LogInformation("Ensuring the database is created");
                await context.Database.EnsureCreatedAsync();

                //logger.LogInformation("Seeding Roles");
                await addRoleAsync(roleManager, "Admin");
                await addRoleAsync(roleManager, "User");

                //logger.LogInformation("Seeding admin user");

                var adminEmail = "Admin@gmail.com";
                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var adminUser = new User
                    {
                        UserName = adminEmail,
                        NormalizedUserName = adminEmail.ToUpper(),
                        Email = adminEmail,
                        NormalizedEmail = adminEmail.ToUpper(),
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString(),
                    };

                    var result = await userManager.CreateAsync(adminUser, "Admin@123");
                    if (result.Succeeded)
                    {
                        //logger.LogInformation("Assigning admin role to the admin user");
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        //logger.LogError($"Failed to create role: {adminEmail}");
                    }
                }
            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "an error occured while seed the database");
            }
        }

        public static async Task addRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception($"failed to create role '{roleName}' : " +
                        $"{string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
