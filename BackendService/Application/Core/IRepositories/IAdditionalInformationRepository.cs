using BackendService.ViewModel;

namespace BackendService.Application.Core.IRepositories
{
    public interface IAdditionalInformationRepository
    {
        Task<DetailAdditionalInformation> GetHelper(string plateNumber);
    }
}
