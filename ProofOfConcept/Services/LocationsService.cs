using ProofOfConcept.Models;
using System.Text.Json;

namespace ProofOfConcept.Services
{
    public interface ILocationsService
    {
        Task<List<Locations>> GetAllFoodPlaces(string amenity, double? latitude, double? longitude);
    }
    public class LocationsService : ILocationsService
    {
        private static readonly string OverpassUrl = "https://overpass-api.de/api/interpreter";
        private string GenerateOverpassQuery(string amenity, double? latitude = null, double? longitude = null, int radiusMeters = 3000)
        {
            // If coordinates are provided, use radius-based search
            if (latitude.HasValue && longitude.HasValue)
            {
                return @$"[out:json];
                   (
                     node[""amenity""=""{amenity}""](around:{radiusMeters},{latitude},{longitude});
                     relation[""amenity""=""{amenity}""](around:{radiusMeters},{latitude},{longitude});
                   );
                   out center;";
            }

            // Default to Bucharest area search if no coordinates provided
            return @$"[out:json];
               area[""ISO3166-2""=""RO-B""]->.searchArea;
               (
                 node[""amenity""=""{amenity}""](area.searchArea);
                 relation[""amenity""=""{amenity}""](area.searchArea);
               );
               out center;";
        }
        private readonly HttpClient _httpClient;
        public LocationsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Locations>> GetAllFoodPlaces(string amenity, double? latitude, double? longitude)
        {
            string query;
            if (longitude is not null && latitude is not null)
                query = GenerateOverpassQuery(amenity, latitude, longitude);
            else
                query = GenerateOverpassQuery(amenity);

            // Prepare the request with the Overpass query
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("data", query)
            });

            // Send the request to Overpass API
            var response = await _httpClient.PostAsync(OverpassUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidDataException($"{(int)response.StatusCode} - Failed to retrive data from the OverpassAPI");
            }

            // Parse the JSON response
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<OverpassApiResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Prepare a simplified response
            var locations = new List<Locations>();

            foreach (var element in data.Elements)
            {
                locations.Add(new Locations
                {
                    Id = element.Id,
                    Type = element.Type,
                    Lat = element.Lat,
                    Lon = element.Lon,
                    Tags = element.Tags
                });
            }

            return locations;
        }
    }
}
