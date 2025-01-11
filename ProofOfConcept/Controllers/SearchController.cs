using FuzzySharp;
using Microsoft.AspNetCore.Mvc;
using ProofOfConcept.Services;

namespace ProofOfConcept.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SearchController : BaseController
    {
        private readonly ISerpService _serpService;
        private readonly ILocationsService _locationsService;
        public SearchController(ISerpService serpService, ILocationsService locationsService)
        {
            _serpService = serpService;
            _locationsService = locationsService;
        }

        [HttpGet]
        [Route("ProcessLocationsAndFindMatches")]
        public async Task<IActionResult> ProcessLocationsAndFindMatches(double logitudeUser, double latitudeUser, string userInput, string selectedAmenity)
        {
            var locations = await _locationsService.GetAllFoodPlaces(selectedAmenity, latitudeUser, logitudeUser);

            locations = locations.Take(5).ToList(); // this is only for testing rn

            foreach (var place in locations)
            {
                var location = await _serpService.SearchForPlaceAsync(place.Lon, place.Lat, place.Tags["name"]);//poate nu e name aici trb vazut pe overpass

                if (location.data_id != null)
                {
                    var photosGroup = await _serpService.GetMenuPhotosForLocationAsync(location.data_id);

                    if (photosGroup.Count > 0)
                    {
                        var menuFiles = await _serpService.DownloadPhotosLocally(photosGroup);

                        foreach (var menuEntry in menuFiles)
                        {
                            //apel ocr

                            var resultedText = "temp";

                            var isValid = await DoesMenuContainUserInput(resultedText, userInput);

                            if (!isValid)
                            {
                                locations.Remove(place);
                            }
                        }
                    }
                }
            }
            return Ok(locations);
        }

        private async Task<bool> DoesMenuContainUserInput(string userInput, string menuContent)
        {
            string[] words = menuContent.Split(new[] { ' ', '\n', '\r', ',', '.', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);

            //maybe needs more optimization 
            foreach (string word in words)
            {
                if (Fuzz.Ratio(word, userInput) >= 90)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
