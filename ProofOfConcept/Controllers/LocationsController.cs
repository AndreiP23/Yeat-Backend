using Microsoft.AspNetCore.Mvc;
using ProofOfConcept.Services;

namespace ProofOfConcept.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationsService _locationsService;

        public LocationsController(ILocationsService locationsService)
        {
            _locationsService = locationsService;
        }

        [HttpPost]
        [Route("TestGetLocations")]
        public async Task<IActionResult> TestGetLocations([FromBody] string amenity)
        {
            return Ok(await _locationsService.GetAllFoodPlaces(amenity));
        }
    }
}
