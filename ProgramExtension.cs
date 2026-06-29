using GymManagment.DAL.Context;
using GymManagment.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.PL
{
    public static class ProgramExtension
    {
        public static async Task MigrateAndSeedDataAsync(this WebApplication app)
        {
            // seeding
            // scope => unmanaged resource
            var scope = app.Services.CreateScope();

            // GymDbContext
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            // looger
            var looger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            // check if there is any pending migration or not
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await dbContext.Database.MigrateAsync(); // aplay => Update-Database  
            }

            // folder path
            var seedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeeding.SeedAsync(dbContext, seedFolderPath, looger);
            await IdentityDataSeeding.SeedIdentityData(roleManager, userManager, looger);
        }
    }
}
