namespace BackendService.Dtos
{
    public class VehicleTempDto
    {
        public VehicleTemp? Data { get; set; }
    }

    public class VehicleTemp
    {
        public VehicleTemp()
        {
            Chambers = new List<Chamber>();
        }

        public string? PlateNumber { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public List<Chamber> Chambers { get; set; }
    }

    public class Chamber
    {
        public Chamber()
        {
            Temperature = new List<Temperature>();
        }
        public List<Temperature> Temperature { get; set; }
    }

    public class Temperature
    {
        public double? TempMax { get; set; }
        public double? TempMin { get; set; }
        public string? TempName { get; set; }
        public double? TempValue { get; set; }
    }
}
