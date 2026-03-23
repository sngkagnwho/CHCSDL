namespace doan.Models
{
    public class TriggerInfo
    {
        public string TriggerName { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public string TriggeringEvent { get; set; } = string.Empty;
        public string Status { get; set; } = "ENABLED";
        public string TriggerBody { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TriggerType { get; set; } = string.Empty;
    }
}
