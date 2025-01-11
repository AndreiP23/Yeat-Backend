using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProofOfConcept.Services;

namespace ProofOfConcept.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LocationsController : BaseController
    {
        private readonly ILocationsService _locationsService;

        public LocationsController(ILocationsService locationsService)
        {
            _locationsService = locationsService;
        }

        [HttpPost]
        [EnableCors]
        [Route("TestGetLocations")]
        public async Task<IActionResult> TestGetLocations([FromQuery]string amenity, [FromQuery] double lat, [FromQuery] double lng)
        {
            var locations = await _locationsService.GetAllFoodPlaces(amenity, lat, lng);
            return Ok(locations);
        }
    }
}
