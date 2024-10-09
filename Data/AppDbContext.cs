using Microsoft.EntityFrameworkCore;
using texasgym_backend.Models;

namespace texasgym_backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Fichas> Fichas { get; set; }
        public DbSet<Treinos> Treinos { get; set; }
    }
}
