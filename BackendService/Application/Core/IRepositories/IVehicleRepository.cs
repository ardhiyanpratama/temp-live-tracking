using BackendService.Dtos;
using BackendService.ViewModel;
using CustomLibrary.Helper;

namespace BackendService.Application.Core.IRepositories
{
    public interface IVehicleRepository
    {
        Task<ResponseBaseViewModel> SubmitVehicleTemp(VehicleTempDto vehicleTempDto);
        Task<List<VehicleTempChamberViewModel>> GetLatestUpdate();
    }
}
