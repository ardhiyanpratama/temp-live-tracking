using BackendService.Application.Core.IRepositories;
using BackendService.Data;
using BackendService.Dtos;
using BackendService.ViewModel;
using CustomLibrary.Adapter;
using CustomLibrary.Exceptions;
using CustomLibrary.Helper;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ILoggerAdapter<VehicleController> _logger;

        public VehicleController(ApplicationDbContext context
            ,ILogger<VehicleController> logger
            ,IVehicleRepository vehicleRepository)
        {
            _context = context;
            _vehicleRepository = vehicleRepository;
            _logger = new LoggerAdapter<VehicleController>(logger);
        }

        [HttpPost]
        public async Task<ActionResult> Post(
        [FromBody] VehicleTempDto input,
            CancellationToken cancellationToken)
        {
            var result = await _vehicleRepository.SubmitVehicleTemp(input);

            if (result.IsError)
            {
                throw new AppException(ResponseMessageExtensions.Database.WriteFailed);
            }

            return this.OkResponse(ResponseMessageExtensions.Database.WriteSuccess);
        }

        [HttpGet]
        public async Task<ActionResult<List<VehicleTempChamberViewModel>>> GetLatestUpdate(
            CancellationToken cancellationToken)
        {
            var result = await _vehicleRepository.GetLatestUpdate();

            return Ok(result);
        }
    }
}
