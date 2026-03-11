using Microsoft.EntityFrameworkCore;
using ConsultorioAPI.Models;
namespace ConsultorioAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Paciente> Pacientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Paciente>().HasIndex(p => p.Email).IsUnique();
            modelBuilder.Entity<Paciente>().HasIndex(p => p.CPF).IsUnique();
        }
    }
}
