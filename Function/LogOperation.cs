using texasgym_backend.Data;
using texasgym_backend.Models;
using System.Threading.Tasks;

namespace texasgym_backend.Function
{
    public class LogFunction
    {
        private readonly AppDbContext _context;

        public LogFunction(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogOperation(string operationType, string tableName, int? recordId, int? userId, string status, string details = "")
        {
            // Criação de um novo registro de log
            var log = new Log
            {
                OperationType = operationType,
                TableName = tableName,
                RecordId = recordId ?? 0, // Use "0" caso o recordId seja nulo
                UserId = userId ?? 0,     // Use "0" caso o userId seja nulo
                Status = status,
                Timestamp = DateTime.Now,
                Details = details
            };

            // Adicionar log no banco de dados
            _context.Logs.Add(log);
            await _context.SaveChangesAsync(); // Salvar as alterações no banco
        }
    }
}
