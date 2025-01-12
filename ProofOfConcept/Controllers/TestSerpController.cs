using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProofOfConcept.Services;

namespace ProofOfConcept.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestSerpController : BaseController
    {
        private readonly ISerpService _serpService;
        public TestSerpController(ISerpService serpService)
        {
            _serpService = serpService;
        }

        [HttpGet]
        [Route("TestGetPhotos")]
        public async Task<IActionResult> TestGetPhotos()
        {
            string dataId = "";

            return Ok(await _serpService.GetMenuPhotosForLocationAsync(dataId));
        }
        [HttpGet]
        [Route("TestGetLocation")]
        public async Task<IActionResult> TestGetLocation()
        {
            double logitude = 26.0984804;
            double latitude = 44.4346528;
            string name = "Pizza Hut Bucuresti Universitate";

            return Ok(await _serpService.SearchForPlaceAsync(logitude, latitude, name));
        }

        [HttpGet]
        [Route("TestDownloadPhotos")]
        public async Task TestDownloadPhotos()
        {
            double logitude = 26.0984804;
            double latitude = 44.4346528;
            string name = "Pizza Hut Bucuresti Universitate";

            var location = await _serpService.SearchForPlaceAsync(logitude, latitude, name);

            if(location.place_results.data_id != null)
            {
                var photosGroup = await _serpService.GetMenuPhotosForLocationAsync(location.place_results.data_id);

                if (photosGroup.Count > 0)
                {
                    await _serpService.DownloadPhotosLocally(photosGroup);
                }
            }
        }
    }
}
