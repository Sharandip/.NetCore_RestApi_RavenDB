using AGDATAApi.Models;
using AGDATAApi.RavenDB;
using AGDATAApi.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AGDATAApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {   
        ILocationService locationService;
       
        public LocationsController(ILocationService locationService)
        {
            this.locationService = locationService;
            
        }

        [HttpGet]

        public async Task<IActionResult> Get()
        {
            List<Location> locations = await this.locationService.GetAllLocations();
            return Ok(locations);

        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> Get(string id)
        {
            if (await locationService.IsLocationExists(id))
            {
                Location location = await locationService.GetLocation(id);
                return Ok(location);
            }
            else
                return NotFound("No location found with id = " + id);
        }
        

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Location location) 
        {
            if (!await locationService.IsDuplicateNameExists(location.Name))
            {
                bool response = await this.locationService.AddLocation(location);
                return StatusCode(StatusCodes.Status201Created);
            }
            else
                return BadRequest("Location with same name already exists.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Location location)
        {
            if (await locationService.IsLocationExists(id))
            {
                bool response = await this.locationService.UpdateLocation(id, location);
                return Ok("Record updated successfully!");
            }
            else
                return NotFound("No location found with id = " + id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (await locationService.IsLocationExists(id))
            {
                bool response = await this.locationService.DeleteLocation(id);
                return Ok("Record deleted!");
            }
            else
                return NotFound("No location found with id = " + id);
        }
    }
}
