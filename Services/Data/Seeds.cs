using Microsoft.AspNetCore.Identity;
using myuzbekistan.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myuzbekistan.Services;

public static class Seeds
{
    // ✅ Метод для создания ролей
    public static async Task SeedRolesAsync(RoleManager<IdentityRole<long>> roleManager)
    {
        string[] roles = new[] { "User", "Admin" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<long>(role));
            }
        }
    }

    // ✅ Метод для создания администратора
    public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
    {
        var adminEmail = "travelAdmin@example.com";
        var adminUsername = "travelAdmin@example.com";
        var adminPassword = "1q2w3e4r5t6y!QAZ";

        var existingAdmin = await userManager.FindByNameAsync(adminUsername);
        if (existingAdmin == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminUsername,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine("✅ Администратор travelAdmin успешно создан!");
            }
            else
            {
                Console.WriteLine("❌ Ошибка при создании администратора:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Description}");
                }
            }
        }
        else
        {
            Console.WriteLine("ℹ️ Администратор travelAdmin уже существует.");
        }
    }


    // ✅ Метод для создания ролей
    public static void SeedAboutContent(AppDbContext context)
    {
        if (!context.Categories.Any(x => x.Name == "About Uzbekistan" || x.Name == "Useful tips"))
        {
            long aboutCategoryId = context.Contents.Max(x => x.Id) + 1;

            List<CategoryEntity> aboutCategories =
            [
                new() {
                Id = aboutCategoryId,
                Name = "About Uzbekistan",
                Locale = "en",
                ViewType = ViewType.More,
                Fields =  8208
            },
            new() {
                Id = aboutCategoryId,
                Name = "Об Узбекистане",
                ViewType = ViewType.More,
                Locale = "ru",
                Fields =  8208
            },
            new() {
                Id = aboutCategoryId,
                Name = "O'zbekiston haqida",
                ViewType = ViewType.More,
                Locale = "uz",
                Fields =  8208
            },
            new() {
                Id = aboutCategoryId + 1,
                Name = "Useful tips",
                Locale = "en",
                ViewType = ViewType.More,
                Fields =  8208
            },
            new() {
                Id = aboutCategoryId + 1,
                Name = "Полезные советы",
                Locale = "ru",
                ViewType = ViewType.More,
                Fields =  8208
            },
            new() {
                Id = aboutCategoryId + 1,
                Name = "Foydali maslahatlar",
                Locale = "uz",
                ViewType = ViewType.More,
                Fields =  8208
            },

        ];


            context.AddRange(aboutCategories);
            context.SaveChanges();
        }

    }


}
