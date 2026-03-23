namespace doan.Models
{
    public class ProcedureInfo
    {
        public string ProcedureName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ProcedureParameter> Parameters { get; set; } = new();
        public string Category { get; set; } = string.Empty;
        public string Icon { get; set; } = "⚙️";
    }

    public class ProcedureParameter
    {
        public string Name { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string Direction { get; set; } = "IN";
        public string Description { get; set; } = string.Empty;
    }
}
