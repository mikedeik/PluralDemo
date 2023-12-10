using AutoMapper;
using PluralDemo.Entities;
using PluralDemo.Models;

namespace PluralDemo.Profiles {
    public class PointOfInterestProfile: Profile {

        public PointOfInterestProfile() {
            CreateMap<PointOfInterest, PointOfInterestDto>();
            CreateMap<PointOfInterestCreateDto, PointOfInterest>();
            CreateMap<PointOfInterest, PointOfInterestCreateDto>();
        }
    }
}
