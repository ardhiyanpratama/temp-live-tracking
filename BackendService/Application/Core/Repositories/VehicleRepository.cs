using BackendService.Application.Core.IRepositories;
using BackendService.Data;
using BackendService.Data.Domain;
using BackendService.Dtos;
using BackendService.ViewModel;
using CustomLibrary.Helper;
using Microsoft.EntityFrameworkCore;
using static CustomLibrary.Helper.ResponseMessageExtensions;

namespace BackendService.Application.Core.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public VehicleRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<VehicleTempChamberViewModel>> GetLatestUpdate()
        {
            var updatedList = await _applicationDbContext.Vehicles
                .Include(x => x.VehicleChambers)
                .GroupBy(x => x.PlateNumber)
                .Select(g => g.OrderByDescending(c => c.CreatedAt).First())
                .ToListAsync();

            var result = new List<VehicleTempChamberViewModel>();

            foreach (var item in updatedList)
            {
                var tempList = new VehicleTempChamberViewModel()
                {
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    PlateNumber = item.PlateNumber
                };

                foreach (var temperatur in item.VehicleChambers.Select((value, index) => new { value, index }))
                {
                    if (temperatur.index == 0)
                    {
                        tempList.TempOneValue = temperatur.value.TempValue;
                    }
                    else
                    {
                        tempList.TempTwoValue = temperatur.value.TempValue;
                    }
                }

                result.Add(tempList);
            }

            return result;
        }

        public async Task<ResponseBaseViewModel> SubmitVehicleTemp(VehicleTempDto vehicleTempDto)
        {
            var response = new ResponseBaseViewModel();
            await using var transaction = _applicationDbContext.Database.BeginTransaction();
            try
            {
                var vehicle = new Vehicle()
                {
                    Latitude = vehicleTempDto?.Data?.Latitude,
                    Longitude = vehicleTempDto?.Data?.Longitude,
                    PlateNumber = vehicleTempDto?.Data?.PlateNumber,
                    CreatedAt = DateTimeOffset.Now,
                };

                await _applicationDbContext.Vehicles.AddAsync(vehicle);

                if (vehicleTempDto?.Data?.Chambers.Count() > 0)
                {
                    var chamberTempList = new List<VehicleChamber>();

                    foreach (var item in vehicleTempDto.Data.Chambers)
                    {
                        foreach (var temperature in item.Temperature)
                        {
                            var chamberTemp = new VehicleChamber()
                            {
                                TempMax = temperature.TempMax,
                                TempMin = temperature.TempMin,
                                TempName = temperature.TempName,
                                TempValue = temperature.TempValue,
                                VehicleId = vehicle.Id,
                                CreatedAt = DateTimeOffset.Now
                            };

                            chamberTempList.Add(chamberTemp);
                        }
                    }

                    await _applicationDbContext.VehicleChambers.AddRangeAsync(chamberTempList);
                }

                transaction.Commit();
                await _applicationDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.IsError = true;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
