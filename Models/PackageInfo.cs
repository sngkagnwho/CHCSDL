namespace doan.Models
{
    public class PackageInfo
    {
        public string PackageName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ProcedureInfo> Procedures { get; set; } = new();
        public List<FunctionInfo> Functions { get; set; } = new();
        public string Status { get; set; } = "VALID";
        public DateTime LastModified { get; set; } = DateTime.Now;
    }
}
