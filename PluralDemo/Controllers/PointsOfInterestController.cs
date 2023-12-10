using Microsoft.AspNetCore.Mvc;
using PluralDemo.Models;
using Microsoft.AspNetCore.JsonPatch;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PluralDemo.Controllers
{
    [Route("api/cities/{cityId}/pois")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {

        private readonly ILogger<PointsOfInterestController> _logger;

        public PointsOfInterestController(
            ILogger<PointsOfInterestController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterst(int cityId)
        {
            try
            {

                var city = CityDataStore.Current.Cities
                .FirstOrDefault<CityDto>(c => c.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"City with Id {cityId} was not found");
                    return NotFound(new { message = "City was not found" });
                }
                return Ok(city.PointsOfInterest);
            } catch (Exception ex)
            {
                _logger.LogCritical($"Exception when trying to get POIs for city with Id {cityId}", ex);
                return StatusCode(500, "An exception occured");
            }
            
        }


        [HttpGet("{poiId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int poiId)
        {
            var city = CityDataStore.Current.Cities
                .FirstOrDefault<CityDto>(c => c.Id == cityId);

            if (city == null)
            {
                _logger.LogInformation($"City with id {cityId} was not found!");
                return NotFound(new { message = "City was not Found" });
            }

            var poi = city.PointsOfInterest
                .FirstOrDefault(p => p.Id == poiId);

            if (poi == null)
            {
                return NotFound(new { message = "Point of Interest was not Found" });
            }

            return Ok(poi);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(
            int cityId,
            PointOfInterestCreateDto poi)
        {
            var city = CityDataStore.Current.Cities
                .FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound(new { message = "City was not Found" });
            }

            // dump way
            int newId = CityDataStore.Current.Cities
                .SelectMany(c => c.PointsOfInterest).Max(p => p.Id) + 1;



            var newPoi = new PointOfInterestDto
            {
                Id = newId,
                Name = poi.Name
            };

            city.PointsOfInterest.Add(newPoi);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId,
                    poiId = newId
                },
                newPoi);
        }

        [HttpPut("{poiId}")]
        public ActionResult<PointOfInterestDto> UpdatePointOfInterest(
           int cityId,
           int poiId,
           PointOfInterestCreateDto newPoi)
        {
            var city = CityDataStore.Current.Cities
                .FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound(new { message = "City was not Found" });
            }

            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(p => p.Id == poiId);

            if (pointOfInterestFromStore == null)
            {
                return NotFound(new { message = "Poi was not Found" });
            }


            pointOfInterestFromStore.Name = newPoi.Name;

            return NoContent();
        }

        [HttpPatch("{poiId}")]
        public ActionResult<PointOfInterestDto> PatchPointOfInterest(
            int cityId,
            int poiId,
            JsonPatchDocument<PointOfInterestCreateDto> patchDocument)
        {
            var city = CityDataStore.Current.Cities
                .FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound(new { message = "City was not Found" });
            }

            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(p => p.Id == poiId);

            if (pointOfInterestFromStore == null)
            {
                return NotFound(new { message = "Poi was not Found" });
            }

            var poiToUpdate = new PointOfInterestCreateDto
            {
                Name = pointOfInterestFromStore.Name
            };

            patchDocument.ApplyTo(poiToUpdate, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!TryValidateModel(poiToUpdate))
            {
                return BadRequest();
            }

            pointOfInterestFromStore.Name = poiToUpdate.Name;

            return NoContent();

        }

        [HttpDelete("{poiId}")]
        public ActionResult<PointOfInterestDto> UpdatePointOfInterest(
            int cityId,
            int poiId)
        {
            var city = CityDataStore.Current.Cities
               .FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound(new { message = "City was not Found" });
            }

            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(p => p.Id == poiId);

            if (pointOfInterestFromStore == null)
            {
                return NotFound(new { message = "Poi was not Found" });
            }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);

            return NoContent();

        }


    }
}

