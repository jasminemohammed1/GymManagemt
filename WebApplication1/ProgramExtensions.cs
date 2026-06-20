using GymManagement.DAL.DataSeading;
using GymManagement.DAL.Models;
using GYMProject.DBContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;

namespace GymManagement.PL
{
    public  static class ProgramExtensions
    {
        public static async  Task MigrateAndSeadDataFilesAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<GymDBContext>();
            var usermanager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var pendingmigrations = await dbcontext.Database.GetPendingMigrationsAsync();
            var log = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            if (pendingmigrations.Any())
            {
                log.LogInformation($"Applying {pendingmigrations.Count()} Pending Migration ");
                dbcontext.Database.Migrate();
            }
            var Folderpath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeading.SaedData(dbcontext, Folderpath, log);
            await IdentityDataSeading.SeadIdentityAsync(usermanager, roleManager, log);

        }
    }
}
