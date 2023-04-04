using AGDATAApi.Models;

namespace AGDATAApi.Services
{
    public interface ILocationService
    {
        public Task<List<Location>> GetAllLocations();
        public Task<Location> GetLocation(string id);
        public Task<bool> AddLocation(Location location);
        public Task<bool> UpdateLocation(string id, Location location);
        public Task<bool> DeleteLocation(string id);

        public Task<bool> IsLocationExists(string id);
        public Task<bool> IsDuplicateNameExists(string name);

    }
}
