using GymManagement.DAL.DataSeading;
using GYMProject.DBContexts;
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
            var pendingmigrations = await dbcontext.Database.GetPendingMigrationsAsync();
            var log = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            if (pendingmigrations.Any())
            {
                log.LogInformation($"Applying {pendingmigrations.Count()} Pending Migration ");
                dbcontext.Database.Migrate();
            }
            var Folderpath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeading.SaedData(dbcontext, Folderpath, log);

        }
    }
}
