using CustomLibrary.Helper;

namespace BackendService.Data.Domain
{
    public class Vehicle:EntityBase
    {
        public string? PlateNumber { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
    }
}
