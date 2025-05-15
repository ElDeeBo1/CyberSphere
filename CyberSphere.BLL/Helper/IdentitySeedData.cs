using CyberSphere.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Helper
{
    public class IdentitySeedData
    {
        public static async Task SeedAdminUser(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            Console.WriteLine($"[SeedAdminUser] Starting seeding...");
            Console.WriteLine($"[SeedAdminUser] Admin Email from config param: {configuration["AdminUser:Email"]}");

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminEmail = configuration["AdminUser:Email"];
            string adminUserName = configuration["AdminUser:UserName"];
            string adminPassword = configuration["AdminUser:Password"];
            string adminRole = "Admin";

            Console.WriteLine($"[SeedAdminUser] Admin Email: {adminEmail}");
            Console.WriteLine($"[SeedAdminUser] Admin UserName: {adminUserName}");
            Console.WriteLine($"[SeedAdminUser] Admin Password: {adminPassword}");


            // Check if role exists
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // Check if admin user exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    Email = adminEmail,
                    UserName = adminUserName,
                    EmailConfirmed = true // optionally confirm email by default
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            // Assign admin role if not assigned
            if (!await userManager.IsInRoleAsync(adminUser, adminRole))
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
    }
}
