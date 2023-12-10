using Microsoft.EntityFrameworkCore;
using PluralDemo.DbContexts;
using PluralDemo.Entities;

namespace PluralDemo.Services {
    public class CityInfoRepository : ICityInfoRepository {

        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context) {
            
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync() {
            
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync();

        }

        public async Task<City?> GetCityAsync(int cityId, bool includePois) {

            if(includePois) {
                return await _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId)
                    .FirstOrDefaultAsync();
            }

            return await _context.Cities
                .FirstOrDefaultAsync(c => c.Id == cityId);
        }

        public async Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int poiId) {


            return await _context.PointOfInterests
                .Where(p => p.CityId == cityId && p.Id == poiId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CityExistsAsync(int cityId) {

            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId) {
            
            return await _context.PointOfInterests
                .Where(p => p.CityId == cityId).ToListAsync();
        }

        public async Task AddPointOfInterestAsync(int cityId, PointOfInterest pointOfInterest) {

            var city = await GetCityAsync(cityId, false);

            city?.PointsOfInterest.Add(pointOfInterest);

        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest) {
            _context.PointOfInterests.Remove(pointOfInterest);
        }

        public async Task<bool> SaveChangesAsync() {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
