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
    }
}
