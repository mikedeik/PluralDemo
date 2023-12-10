using Microsoft.AspNetCore.Mvc;
using PluralDemo.Models;
using Microsoft.AspNetCore.JsonPatch;
using PluralDemo.Services;
using AutoMapper;
using PluralDemo.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PluralDemo.Controllers
{
    [Route("api/cities/{cityId}/pois")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {

        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly ISendMail _sendMailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(
            ILogger<PointsOfInterestController> logger,
            ISendMail sendMail,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper,
            CityDataStore citiesDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sendMailService = sendMail ?? throw new ArgumentNullException(nameof(sendMail));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterst(int cityId)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId)) {

                _logger.LogInformation($"City with id {cityId} was not found!");

                return NotFound(new {message = "City was not Found"});
            }
            var pois = await _cityInfoRepository.GetPointsOfInterestAsync(cityId);

            return Ok(_mapper.Map<PointOfInterestDto>(pois));

            
        }


        [HttpGet("{poiId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int poiId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {

                _logger.LogInformation($"City with id {cityId} was not found!");

                return NotFound(new { message = "City was not Found" });
            }

            var poi = await _cityInfoRepository.GetPointOfInterestAsync(cityId, poiId);

            if(poi == null) {
                _logger.LogInformation($"POI with id {poiId} was not found!");

                return NotFound(new { message = "Point of Interest was not Found" });
            }

            return Ok(_mapper.Map<PointOfInterestDto>(poi));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
            int cityId,
            PointOfInterestCreateDto poi) {
           
            if(!await _cityInfoRepository.CityExistsAsync(cityId)) {

                _logger.LogInformation($"City with id {cityId} was not found!");
                return NotFound(new { message = "City was not Found" });
            }

            var finalPoi = _mapper.Map<PointOfInterest>(poi);
            
            await _cityInfoRepository.AddPointOfInterestAsync(cityId, finalPoi);

            await _cityInfoRepository.SaveChangesAsync();

            var createdPoi = _mapper.Map<PointOfInterestDto>(finalPoi);

            return CreatedAtRoute("GetPointOfInterest",
                new {
                    cityId = cityId,
                    poiId = createdPoi.Id
                },
                createdPoi);
        }

        [HttpPut("{poiId}")]
        public async Task<ActionResult<PointOfInterestDto>> UpdatePointOfInterest(
           int cityId,
           int poiId,
           PointOfInterestCreateDto newPoi) {
            

            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {
                _logger.LogInformation($"City with id {cityId} was not found!");
                return NotFound(new { message = "City was not Found" });
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestAsync(cityId, poiId);

            if (pointOfInterestEntity == null) {
                _logger.LogInformation($"POI with id {poiId} was not found!");

                return NotFound(new { message = "Point of Interest was not Found" });
            }
            
            // we can pass as params 
            _mapper.Map(newPoi, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{poiId}")]
        public async Task<ActionResult<PointOfInterestDto>> PatchPointOfInterest(
            int cityId,
            int poiId,
            JsonPatchDocument<PointOfInterestCreateDto> patchDocument) {


            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {
                _logger.LogInformation($"City with id {cityId} was not found!");
                return NotFound(new { message = "City was not Found" });
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestAsync(cityId, poiId);

            if (pointOfInterestEntity == null) {
                _logger.LogInformation($"POI with id {poiId} was not found!");

                return NotFound(new { message = "Point of Interest was not Found" });
            }

            var poiToUpdate = _mapper.Map<PointOfInterestCreateDto>(pointOfInterestEntity);


            patchDocument.ApplyTo(poiToUpdate, ModelState);

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(poiToUpdate)) {
                return BadRequest(ModelState);
            }
            _mapper.Map(poiToUpdate, pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{poiId}")]
        public async Task<ActionResult<PointOfInterestDto>> UpdatePointOfInterest(
            int cityId,
            int poiId) {

            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {
                _logger.LogInformation($"City with id {cityId} was not found!");
                return NotFound(new { message = "City was not Found" });
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestAsync(cityId, poiId);

            if (pointOfInterestEntity == null) {
                _logger.LogInformation($"POI with id {poiId} was not found!");

                return NotFound(new { message = "Point of Interest was not Found" });
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            _sendMailService.Send("POI Deleted", $"Point of interest  {pointOfInterestEntity.Name} with Id {pointOfInterestEntity.Id} was deleted!");
            return NoContent();

        }


    }
}

