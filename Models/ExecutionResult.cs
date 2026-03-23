namespace doan.Models
{
    public class ExecutionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, object> OutputValues { get; set; } = new();
        public string ProcedureName { get; set; } = string.Empty;
        public DateTime ExecutedAt { get; set; } = DateTime.Now;
        public long ExecutionTimeMs { get; set; }
    }
}
