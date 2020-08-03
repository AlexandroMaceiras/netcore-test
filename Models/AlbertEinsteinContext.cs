using Microsoft.EntityFrameworkCore;

namespace AlbertEinstein.Models
{
    public class AlbertEinsteinContext : DbContext
    {
        public AlbertEinsteinContext(DbContextOptions<AlbertEinsteinContext> options)
            : base(options)
        {
        }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Exame> Exames { get; set; }

        /* 
        
        // Não estou usando o overrride no modelo porque eu quero que as tabelas apareçam com os nomes no plural 
        // pois contém mais de um registro.
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medico>().ToTable("Medico");
            modelBuilder.Entity<Paciente>().ToTable("Paciente");
            modelBuilder.Entity<Consulta>().ToTable("Consulta");
            modelBuilder.Entity<Exame>().ToTable("Exame");
        }
        
        */
    }
}