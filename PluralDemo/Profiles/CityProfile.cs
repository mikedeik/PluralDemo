using AutoMapper;

namespace PluralDemo.Profiles {
    public class CityProfile: Profile {

        public CityProfile() {
            CreateMap<Entities.City, Models.CityWithoutPoisDto>();
            CreateMap<Entities.City, Models.CityDto>();
        }
    }
}
