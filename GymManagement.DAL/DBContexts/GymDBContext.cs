using GymManagement.DAL.Models;
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
           modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly()); 
        }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }  
        public DbSet<Sessions>Sessions { get; set; }
        public DbSet<Trainer> Trainers { get; set; }  
        public DbSet<Member> Members { get; set; }


    }
}
