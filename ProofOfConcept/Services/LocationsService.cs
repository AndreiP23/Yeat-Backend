namespace ProofOfConcept.Services
{
    public interface ILocationsService
    {

    }
    public class LocationsService : ILocationsService   
    {
        private readonly HttpClient _httpClient;
        public LocationsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
