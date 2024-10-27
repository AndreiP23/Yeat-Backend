﻿using ProofOfConcept.Models;
using System.Text.Json;

namespace ProofOfConcept.Services
{
    public interface ILocationsService
    {
        Task<List<Locations>> GetAllFoodPlaces();
    }
    public class LocationsService : ILocationsService
    {
        private static readonly string OverpassUrl = "https://overpass-api.de/api/interpreter";
        private string GenerateOverpassQuery()
        {
            return @"[out:json];
                        area[""ISO3166-2""=""RO-B""]->.searchArea;  // This targets the Bucharest area using Romania's ISO code
                        (
                          node[""amenity""=""restaurant""](area.searchArea);
                          node[""amenity""=""fast_food""](area.searchArea);
                          way[""amenity""=""restaurant""](area.searchArea);
                          way[""amenity""=""fast_food""](area.searchArea);
                          relation[""amenity""=""restaurant""](area.searchArea);
                          relation[""amenity""=""fast_food""](area.searchArea);
                        );
                        out center;";
        }
        private readonly HttpClient _httpClient;
        public LocationsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Locations>> GetAllFoodPlaces()
        {
            // Prepare the request with the Overpass query
            var query = GenerateOverpassQuery();
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
            try
            {
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
            }
            catch (Exception ex)
            {

            }
            

            return locations;
        }
    }
}
