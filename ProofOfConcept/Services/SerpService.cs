using Microsoft.AspNetCore.DataProtection.KeyManagement;
using ProofOfConcept.Models;
using ProofOfConcept.Refit;

namespace ProofOfConcept.Services
{
    public interface ISerpService
    {
        Task<List<Photo>> GetMenuPhotosForLocationAsync(string locationQuery, double? latitude = null, double? longitude = null);
        Task<string> SearchForPlaceAsync(double logitude, double latitude, string name);
    }
    public class SerpService : ISerpService
    {
        private readonly ISerpApi _serpApi;
        private readonly IConfiguration _configuration;

        public SerpService(ISerpApi serpApi, IConfiguration configuration)
        {
            _serpApi = serpApi;
            _configuration = configuration;
        }

        public async Task<List<Photo>> GetMenuPhotosForLocationAsync(string locationQuery, double? latitude = null, double? longitude = null)
        {
            try
            {
                var srpApiKey = _configuration["serp-api-key"];

                if(srpApiKey is null)
                    return new List<Photo>();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     

                var response = await _serpApi.GetLocationMenuPhotosAsync(locationQuery, latitude: latitude, longitude: longitude, apiKey: srpApiKey);

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.Photos ?? new List<Photo>();
                }
                else
                {
                    Console.WriteLine($"Error: {response.Error?.Message}");
                    return new List<Photo>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<Photo>();
            }
        }
        public async Task<string> SearchForPlaceAsync(double logitude, double latitude, string name)
        {
            var engine = "google_maps";
            var location = $"@{latitude},{logitude},10z"; // Latitude, Longitude, Zoom
            var type = "search";
            var srpApiKey = _configuration["serp-api-key"];

            if (srpApiKey is null)
                return string.Empty;

            var response = await _serpApi.SearchLocationAsync(engine, location, type, srpApiKey, name);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
            else
            {
                Console.WriteLine($"Error: {response.RequestMessage}");
                return string.Empty;
            }
        }
    }
}
