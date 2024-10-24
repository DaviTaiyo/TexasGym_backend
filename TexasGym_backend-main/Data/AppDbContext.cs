using Microsoft.EntityFrameworkCore;
using texasgym_backend.Models;

namespace texasgym_backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Fichas> Fichas { get; set; }
        public DbSet<Exercicio> Exercicios { get; set; }
        public DbSet<Treinos> Treinos { get; set; }
        public DbSet<TreinoFicha> TreinoFichas { get; set; }
        public DbSet<Medida> Medidas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relacionamento entre Ficha e Usuario (Um para muitos)
            modelBuilder.Entity<Fichas>()
                .HasOne(f => f.Usuario)
                .WithMany(u => u.Fichas)
                .HasForeignKey(f => f.UsuarioId);

            // Relacionamento entre Exercício e Ficha (Um para muitos)
            modelBuilder.Entity<Exercicio>()
                .HasOne(e => e.Ficha)
                .WithMany(f => f.Exercicios)
                .HasForeignKey(e => e.FichaId);

            // Relacionamento entre TreinoFicha e Ficha (Um para muitos)
            modelBuilder.Entity<TreinoFicha>()
                .HasOne(tf => tf.Ficha)
                .WithMany(f => f.TreinoFichas)
                .HasForeignKey(tf => tf.FichaId);

            // Relacionamento entre TreinoFicha e Treino (Um para muitos)
            modelBuilder.Entity<TreinoFicha>()
                .HasOne(tf => tf.Treino)
                .WithMany(t => t.TreinoFichas)
                .HasForeignKey(tf => tf.TreinoId);
        }
    }
}
