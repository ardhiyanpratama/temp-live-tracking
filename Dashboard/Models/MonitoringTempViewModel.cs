namespace Dashboard.Models
{
    public class MonitoringTempViewModel
    {
        public string? PlateNumber { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? TempOneValue { get; set; }
        public double? TempTwoValue { get; set; }
        public string? LastUpdated { get; set; }
    }
}
