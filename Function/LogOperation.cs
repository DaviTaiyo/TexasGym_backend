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
            var log = new Log
            {
                OperationType = operationType,
                TableName = tableName,
                RecordId = recordId,
                UserId = userId,
                Status = status,
                Timestamp = DateTime.Now,
                Details = details
            };
            await _context.SaveChangesAsync();
        }
    }
}
