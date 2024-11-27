using System;
using System.Threading.Tasks;
using texasgym_backend.Data;
using texasgym_backend.Models;

namespace texasgym_backend.Services
{
    public class LoggingService
    {
        private readonly AppDbContext _context;

        public LoggingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(string operationType, string tableName, int recordId, string details, int? userId = null)
        {
            // Criação do log usando o namespace completo para evitar ambiguidade
            var log = new texasgym_backend.Models.Log
            {
                OperationType = operationType,
                TableName = tableName,
                RecordId = recordId,
                UserId = userId,
                Status = "SUCCESS",
                Timestamp = DateTime.Now,
                Details = details
            };

            try
            {
                _context.Logs.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Tratamento de erro ao salvar o log
                Console.WriteLine($"Erro ao salvar log: {ex.Message}");
            }
        }
    }
}
