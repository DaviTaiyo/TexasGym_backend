using Microsoft.EntityFrameworkCore;
using texasgym_backend.Models;

namespace texasgym_backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ficha> Fichas { get; set; }
        public DbSet<Exercicio> Exercicios { get; set; }
        public DbSet<Treino> Treinos { get; set; }
        public DbSet<Medida> Medidas { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ficha>()
                .HasOne(f => f.Usuario)
                .WithMany(u => u.Fichas)
                .HasForeignKey(f => f.UsuarioId)
                .IsRequired(false);

            modelBuilder.Entity<Log>().ToTable("Logs");
        }
    }
}
