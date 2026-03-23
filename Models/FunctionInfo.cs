namespace doan.Models
{
    public class FunctionInfo
    {
        public string FunctionName { get; set; } = string.Empty;
        public string ReturnType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ProcedureParameter> Parameters { get; set; } = new();
        public string Category { get; set; } = string.Empty;
        public string ExampleCall { get; set; } = string.Empty;
    }
}
