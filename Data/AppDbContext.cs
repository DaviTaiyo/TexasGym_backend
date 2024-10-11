using Microsoft.EntityFrameworkCore;
using texasgym_backend.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Ficha> Fichas { get; set; }
    public DbSet<Exercicio> Exercicios { get; set; }
    public DbSet<Treino> Treinos { get; set; }
    public DbSet<TreinoFicha> TreinosFichas { get; set; }
    public DbSet<Medida> Medidas { get; set; }

}
