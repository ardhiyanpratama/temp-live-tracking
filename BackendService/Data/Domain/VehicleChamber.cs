using CustomLibrary.Helper;

namespace BackendService.Data.Domain
{
    public class VehicleChamber:EntityBase
    {
        public Guid VehicleId { get; set; }
        public double? TempMax { get; set; }
        public double? TempMin { get; set; }
        public string? TempName { get; set; }
        public double? TempValue { get; set; }

        public virtual Vehicle? Vehicle { get; set; }
    }
}
