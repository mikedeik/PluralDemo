using System;
using Microsoft.AspNetCore.Mvc;
using PluralDemo.Models;

namespace PluralDemo.Controllers
{
	[ApiController]
	[Route("api/cities")]
	public class CitiesController: ControllerBase
	{
		private readonly CityDataStore _citiesDataStore;
		public CitiesController(CityDataStore cityDataStore)
		{
            _citiesDataStore = cityDataStore ?? throw new ArgumentNullException(nameof(_citiesDataStore));
        }

		[HttpGet]
		public ActionResult<IEnumerable<CityDto>> GetCities()
		{
			return Ok(_citiesDataStore.Cities);
		}

		[HttpGet("{id}")]
		public ActionResult<CityDataStore> GetCity(int id)
		{
			var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);


			
			return city == null ? NotFound(new { message = "City not found"}) : Ok(city);
            
		}
	}
}

