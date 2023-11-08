using CustomLibrary.Helper;

namespace BackendService.Data.Domain
{
    public class AdditionalInformation:EntityBase
    {
        public string? PlateNumber { get; set; }
        public string? Helper { get; set; }
    }
}
