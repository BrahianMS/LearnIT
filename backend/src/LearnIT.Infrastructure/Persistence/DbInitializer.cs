using LearnIT.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearnIT.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task InitializeAsync(LearnITDbContext context, UserManager<ApplicationUser> userManager)
    {
        // Apply migrations if any
        await context.Database.MigrateAsync();

        // Look for any users.
        if (await userManager.Users.AnyAsync())
        {
            return;   // DB has been seeded
        }

        var user = new ApplicationUser
        {
            UserName = "admin@learnit.com",
            Email = "admin@learnit.com",
            FullName = "Admin User",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(user, "Password123!");
    }
}
