using BackendService.Application.Core.IRepositories;
using BackendService.Data;
using BackendService.ViewModel;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using static MassTransit.ValidationResultExtensions;

namespace BackendService.Application.Core.Repositories
{
    public class AdditionalInformationRepository : IAdditionalInformationRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AdditionalInformationRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<DetailAdditionalInformation> GetHelper(string plateNumber)
        {
            //find helper
            var helper = await _applicationDbContext.AdditionalInformation.FirstOrDefaultAsync(x => x.PlateNumber.ToLower().Trim().Contains(plateNumber.ToLower().Trim()) || x.PlateNumber.ToLower().Trim() == plateNumber.ToLower().Trim())
                .Select(x => x.Helper);

            if (helper is null)
            {
                return null;
            }

            //get temperatur
            var updatedList = await _applicationDbContext.Vehicles
                .Include(x => x.VehicleChambers)
                .Where(x => x.PlateNumber.ToLower() == plateNumber.ToLower())
                .GroupBy(x => x.PlateNumber)
                .Select(g => g.OrderByDescending(c => c.CreatedAt).First())
                .ToListAsync();

            var result = new DetailAdditionalInformation();
            double? tempOneValue = 0;
            double? tempTwoValue = 0;

            result.Helper = helper;

            foreach (var item in updatedList)
            {
                foreach (var temperatur in item.VehicleChambers.Select((value, index) => new { value, index }))
                {
                    if (temperatur.index == 0)
                    {
                        tempOneValue = temperatur.value.TempValue;
                    }
                    else
                    {
                        tempTwoValue = temperatur.value.TempValue;
                    }
                }
            }

            result.Data.TempOneValue = tempOneValue;
            result.Data.TempTwoValue = tempTwoValue;

            return result;

        }
    }
}
