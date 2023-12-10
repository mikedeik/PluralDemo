using System;
using PluralDemo.Models;

namespace PluralDemo
{
	public class CityDataStore
	{
		public List<CityDto> Cities { get; set; }
		public static CityDataStore Current { get; } = new CityDataStore();
		public CityDataStore()
		{
			Cities = new List<CityDto>
			{
				new CityDto
				{
					Id = 1,
					Name = "Athens",
					Description = "Trash City"
				},
				new CityDto
				{
					Id = 2,
					Name = "New York",
					Description = "Feels Good Man"
				},
				new CityDto
				{
					Id = 3,
					Name = "Paris",
					Description = "Been There Done That",

					PointsOfInterest = new List<PointOfInterestDto>()
					{
						new PointOfInterestDto
						{
							Id = 1,
							Name = "The Louvre"
						},
						new PointOfInterestDto
						{
							Id = 2,
							Name = "Eiffel Tower"
						}
					}
                },
            };
		}
	}
}

