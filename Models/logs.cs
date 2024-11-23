namespace texasgym_backend.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string OperationType { get; set; }
        public string TableName { get; set; }
        public int? RecordId { get; set; }
        public int? UserId { get; set; }
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }
    }
}
