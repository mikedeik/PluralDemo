using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PluralDemo.Models;
using PluralDemo.Services;

namespace PluralDemo.Controllers
{
	[ApiController]
	[Route("api/cities")]
	public class CitiesController: ControllerBase
	{

		private readonly ICityInfoRepository _citiesInfoRepository;
		private readonly IMapper _mapper;

		public CitiesController(ICityInfoRepository cityInfoRepository,
			IMapper mapper)
		{
            _citiesInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CityWithoutPoisDto>>> GetCities()
		{
			var citiyEntities = await _citiesInfoRepository.GetCitiesAsync();
		
			return Ok(_mapper.Map<IEnumerable<CityWithoutPoisDto>>(citiyEntities));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCity(int id, bool includePois = false) {
			
			var city = await _citiesInfoRepository.GetCityAsync(id, includePois);

			if(city == null) {
				return NotFound();
			}

			if(includePois) {
				return Ok(_mapper.Map<CityDto>(city));
			}

			return Ok(_mapper.Map<CityWithoutPoisDto>(city));
		}
	}
}

