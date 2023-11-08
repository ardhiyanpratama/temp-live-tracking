using BackendService.Application.Core.IRepositories;
using BackendService.Application.RabbitMq;
using BackendService.Data;
using BackendService.Dtos;
using BackendService.ViewModel;
using CustomLibrary.Adapter;
using CustomLibrary.Exceptions;
using CustomLibrary.Helper;
using MassTransit;
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
        private readonly IProducer _producer;
        private readonly IAdditionalInformationRepository _additionalInformationRepository;

        public VehicleController(ApplicationDbContext context
            ,ILogger<VehicleController> logger
            ,IVehicleRepository vehicleRepository
            ,IProducer producer
            ,IAdditionalInformationRepository additionalInformationRepository)
        {
            _context = context;
            _vehicleRepository = vehicleRepository;
            _logger = new LoggerAdapter<VehicleController>(logger);
            _producer = producer;
            _additionalInformationRepository = additionalInformationRepository;
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

            var additionalInformation = await _additionalInformationRepository.GetHelper(input.Data.PlateNumber);

            _producer.SendDetailMessage(additionalInformation);

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
