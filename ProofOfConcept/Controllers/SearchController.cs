using FuzzySharp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProofOfConcept.Models;
using ProofOfConcept.Services;

namespace ProofOfConcept.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SearchController : BaseController
    {
        private readonly ISearchService _searchService;
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ProcessLocationsAndFindMatches")]
        public async Task<IActionResult> ProcessLocationsAndFindMatches(double logitudeUser, double latitudeUser, string userInput, string selectedAmenity)
        {
            return Ok(await _searchService.ProcessLocation(selectedAmenity,latitudeUser,logitudeUser,userInput));
        }
    }
}
