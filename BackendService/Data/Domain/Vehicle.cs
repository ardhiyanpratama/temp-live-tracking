using CustomLibrary.Helper;

namespace BackendService.Data.Domain
{
    public class Vehicle:EntityBase
    {
        public Vehicle()
        {
            VehicleChambers = new List<VehicleChamber>();
        }
        public string? PlateNumber { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        public virtual ICollection<VehicleChamber> VehicleChambers { get; set; }
    }
}
