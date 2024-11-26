using Microsoft.EntityFrameworkCore;
using texasgym_backend.Models;

namespace texasgym_backend.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Medida> Medidas { get; set; }
        public DbSet<Ficha> Fichas { get; set; }
        public DbSet<Treino> Treinos { get; set; }
        public DbSet<Exercicio> Exercicios { get; set; }
        public DbSet<TreinoExercicio> TreinoExercicios { get; set; } // Adiciona TreinoExercicio

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações adicionais para tabelas intermediárias
            modelBuilder.Entity<TreinoExercicio>()
                .HasOne(te => te.Treino)
                .WithMany(t => t.TreinosExercicios)
                .HasForeignKey(te => te.TreinoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TreinoExercicio>()
                .HasOne(te => te.Exercicio)
                .WithMany(e => e.TreinosExercicios)
                .HasForeignKey(te => te.ExercicioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
