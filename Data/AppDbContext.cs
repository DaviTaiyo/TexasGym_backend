using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public DbSet<TreinoExercicio> TreinoExercicios { get; set; }
        public DbSet<Log> Logs { get; set; } // Adiciona a entidade de Logs

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var logs = new List<Log>();
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                var tableName = entry.Entity.GetType().Name;
                var primaryKey = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
                var recordId = primaryKey?.CurrentValue as int? ?? 0;

                var log = new Log
                {
                    OperationType = entry.State.ToString().ToUpper(), // "ADDED", "UPDATED", "DELETED"
                    TableName = tableName,
                    RecordId = recordId,
                    Status = "SUCCESS",
                    Timestamp = DateTime.Now,
                    Details = $"Operation: {entry.State} on {tableName} - Record ID: {recordId}"
                };

                logs.Add(log);
            }

            if (logs.Any())
                Logs.AddRange(logs);

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
