using System;

namespace texasgym_backend.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string OperationType { get; set; } // CREATE, UPDATE, DELETE
        public string TableName { get; set; } // Nome da tabela (exemplo: "Usuarios")
        public int RecordId { get; set; } // ID do registro afetado
        public int? UserId { get; set; } // ID do usuário responsável pela ação (opcional)
        public string Status { get; set; } // SUCCESS ou ERROR
        public DateTime Timestamp { get; set; } // Data e hora do log
        public string Details { get; set; } // Detalhes adicionais da operação
    }
}
