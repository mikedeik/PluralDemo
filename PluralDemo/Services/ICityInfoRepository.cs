using PluralDemo.Entities;

namespace PluralDemo.Services {

    public interface ICityInfoRepository {

        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityAsync(int cityId, bool includePois);
        Task<bool> CityExistsAsync(int cityId);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int poiId);
        Task AddPointOfInterestAsync(int cityId, PointOfInterest pointOfInterest);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
        Task<bool> SaveChangesAsync();

    }
}
