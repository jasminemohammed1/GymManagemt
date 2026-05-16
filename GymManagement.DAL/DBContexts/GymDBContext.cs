using GYMProject.Configurations;
using GYMProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GYMProject.DBContexts
{
    public class GymDBContext : DbContext
    {

        public GymDBContext(DbContextOptions<GymDBContext> options) : base(options)
        {
            
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;DataBase=GymManagement;Trusted_Connection = True; TrustServerCertificate = True;");

        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Plan>(new PlanConfiguration());
        }
        public DbSet<Plan> Plans { get; set; }

    }
}
