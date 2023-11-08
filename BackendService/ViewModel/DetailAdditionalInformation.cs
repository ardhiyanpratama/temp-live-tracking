using BackendService.Dtos;

namespace BackendService.ViewModel
{
    public class DetailAdditionalInformation
    {
        public DetailAdditionalInformation()
        {
            Data = new DetailTemperature();
        }
        public string? Helper { get; set; }
        public DetailTemperature Data { get; set; }
    }

    public class DetailTemperature
    {
        public double? TempOneValue { get; set; }
        public double? TempTwoValue { get; set; }
    }
}
