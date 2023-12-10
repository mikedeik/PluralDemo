using System;
using Microsoft.AspNetCore.Mvc;
using PluralDemo.Models;

namespace PluralDemo.Controllers
{
	[ApiController]
	[Route("api/cities")]
	public class CitiesController: ControllerBase
	{
		public CitiesController()
		{
		}

		[HttpGet]
		public ActionResult<IEnumerable<CityDto>> GetCities()
		{
			return Ok(CityDataStore.Current.Cities);
		}

		[HttpGet("{id}")]
		public ActionResult<CityDataStore> GetCity(int id)
		{
			var city = CityDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);


			
			return city == null ? NotFound(new { message = "City not found"}) : Ok(city);
            
		}
	}
}

