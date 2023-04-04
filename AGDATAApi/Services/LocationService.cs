using AGDATAApi.Models;
using AGDATAApi.RavenDB;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Memory;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace AGDATAApi.Services
{
    public class LocationService : ILocationService
    {
        private readonly IAsyncDocumentSession dbSession;
        private readonly IMemoryCache _memoryCache;
        const string key = "locations";

        public LocationService(IAsyncDocumentSession dbSession, IMemoryCache memoryCache) {
            this.dbSession = dbSession;
            _memoryCache = memoryCache;
        }

        public async Task<List<Location>> GetAllLocations()
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

            if (_memoryCache.TryGetValue(key, out IList<Location> cacheValue))
                return cacheValue.ToList();
            cacheValue = await this.dbSession.Query<Location>().ToListAsync();
            _memoryCache.Set(key, cacheValue, cacheOptions);
            
            return cacheValue.ToList();
        }


        public async Task<Location> GetLocation(string id)
        {
            Location location = await dbSession.LoadAsync<Location>(id);
            return location;
        }

        public async Task<bool> AddLocation(Location location)
        {
            await dbSession.StoreAsync(location);
            await dbSession.SaveChangesAsync();
            _memoryCache.Remove(key);
            return true;
        }

        public async Task<bool> UpdateLocation(string id, Location location)
        {
            Location locationToUpdate = await dbSession.Query<Location>().FirstOrDefaultAsync(loc => loc.Id == id);

            locationToUpdate.Name = location.Name;
            locationToUpdate.City = location.City;
            locationToUpdate.Province = location.Province;
            locationToUpdate.PostalCode = location.PostalCode;
            locationToUpdate.StreetName = location.StreetName;
            await dbSession.SaveChangesAsync();
            _memoryCache.Remove(key);
            return true;
        }

        public async Task<bool> DeleteLocation(string id)
        {
            dbSession.Delete(id);
            await dbSession.SaveChangesAsync();
            _memoryCache.Remove(key);
            return true;
        }

        public async Task<bool> IsLocationExists(string id)
        {
            bool isLocationExists = false;

            Location location = await dbSession.Query<Location>().FirstOrDefaultAsync(loc => loc.Id == id);
            if (location != null)
                isLocationExists = true;

            return isLocationExists;
        }

        public async Task<bool> IsDuplicateNameExists(string name) {
            bool isDuplocateNameExists = false;

            Location location = await dbSession.Query<Location>().FirstOrDefaultAsync(loc => loc.Name == name);
            if (location != null)
                isDuplocateNameExists = true;

            return isDuplocateNameExists;
        }
    }
}
