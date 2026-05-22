using GymSystem.FluentConfigurations;
using GymSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Context
{
    public class GymDbContet : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = .;Database = GymSystem;Trusted_Connection = True; TrustServerCertificate = True;");
        }

        #region configration

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Plan>(new PlanConfiguration());
        }

        #endregion

        #region Models

        public DbSet<Plan> Plans { get; set; }

        #endregion
    }
}
